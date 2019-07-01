using System;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.API.Controllers {
  public class BulkControllerTest {
    private readonly BulkController _bulkController;
    private readonly KuzzleApiMock _api;

    public BulkControllerTest() {
      _api = new KuzzleApiMock();
      _bulkController = new BulkController(_api.MockedObject);
    }

    [Fact]
    public async void ImportTestSuccess() {

      JObject expected = JObject.Parse(@"{items: [{create: {_id: 'documentId', status: 201}}]}");

      _api.SetResult(JObject.Parse(@"{result: {items: [{create: {_id: 'documentId', status: 201}}]}}"));

      JObject result = await _bulkController.ImportAsync(
        "foo",
        "bar",
        JArray.Parse(@"[]")
        );

      _api.Verify(new JObject {
        {"index", "foo"},
        {"collection", "bar"},
        {"controller", "bulk"},
        {"action", "import"},
        {"body", new JObject {
          {"bulkData", JArray.Parse(@"[]")}
        } }
      });

      Assert.Equal<JObject>(
        expected,
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void mWriteTestSuccess(bool notify) {
      JObject expected = JObject.Parse(@"{hits: [
      {_id: '<documentId>', _source: {}, _version: 2, created: false},
      {_id: '<otherDocumentId>', _source: {}, _version: 1, created: true}],
      total: 2}");

      _api.SetResult(@"{hits: [
      {_id: '<documentId>', _source: {}, _version: 2, created: false},
      {_id: '<otherDocumentId>', _source: {}, _version: 1, created: true}],
      total: 2}");

      JObject result = await _bulkController.mWriteAsync("foo", "bar", JArray.Parse(@"[
          {_id: '<documentId>', body: {}},
          {_id: '<otherDocumentId', body:{}}]"), null, notify);

      _api.Verify(new JObject {
          {"index", "foo"},
          {"collection", "bar"},
          {"controller", "bulk"},
          {"action", "mWrite"},
          {"notify", notify},
          {"body", JObject.Parse(@"{documents: [
          {_id: '<documentId>', body: {}},
          {_id: '<otherDocumentId', body:{}}]}")}
      });
    }
  }
}

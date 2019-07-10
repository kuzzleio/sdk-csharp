using Kuzzle.Tests.Utils;
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
    public async void ImportAsyncTestSuccess() {

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

      Assert.Equal<JObject>(expected, result, new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async void MWriteAsyncTestSuccess(bool notify, bool refresh) {
      JObject expected = JObject.Parse(@"{hits: [
      {_id: '<documentId>', _source: {}, _version: 2, created: false},
      {_id: '<otherDocumentId>', _source: {}, _version: 1, created: true}],
      total: 2}");

      _api.SetResult(@"{result: {hits: [
      {_id: '<documentId>', _source: {}, _version: 2, created: false},
      {_id: '<otherDocumentId>', _source: {}, _version: 1, created: true}],
      total: 2}}");

      JObject result = await _bulkController.MWriteAsync("foo", "bar", JArray.Parse(@"[
          {_id: '<documentId>', body: {}},
          {_id: '<otherDocumentId>', body:{}}]"), refresh, notify);

      JObject verifyQuery = new JObject {
          {"index", "foo"},
          {"collection", "bar"},
          {"controller", "bulk"},
          {"action", "mWrite"},
          {"body", JObject.Parse(@"{documents: [
          {_id: '<documentId>', body: {}},
          {_id: '<otherDocumentId>', body:{}}]}")}
      };

      QueryUtils.HandleRefreshOption(verifyQuery, refresh);
      QueryUtils.HandleNotifyOption(verifyQuery, notify);

      _api.Verify(verifyQuery);

      Assert.Equal<JObject>(expected, result, new JTokenEqualityComparer());
    }
  
    [Theory]
    [InlineData("Some input", "foobar", false, false)]
    [InlineData("", "documentId", false, false)]
    [InlineData("Some input", "foobar", true, false)]
    [InlineData("Some input", "foobar", false, true)]
    public async void WriteAsyncTestSuccess(string documentInput, string documentId, bool notify, bool refresh) {
      JObject expected = JObject.Parse(
      @"{_id: '<documentId>',
      _version: 1,
      created: true,
      _source: {}}");

      _api.SetResult(@"{result: {_id: '<documentId>',
      _version: 1,
      created: true,
      _source: {}}}");

      JObject result = await _bulkController.WriteAsync(
        "foo",
        "bar",
        documentInput,
        documentId,
        refresh,
        notify);

      JObject verifyQuery = new JObject {
            {"index", "foo"},
            {"collection", "bar"},
            {"controller", "bulk"},
            {"action", "write"},
            {"_id", documentId},
            {"body", documentInput}
        };

      QueryUtils.HandleRefreshOption(verifyQuery, refresh);
      QueryUtils.HandleNotifyOption(verifyQuery, notify);

      _api.Verify(verifyQuery);

      Assert.Equal<JObject>(expected, result, new JTokenEqualityComparer());
    }
  }
}


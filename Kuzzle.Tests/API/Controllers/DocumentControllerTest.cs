using System.Collections.Generic;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.API.Controllers {
  public class DocumentControllerTest {
    private readonly DocumentController _documentController;
    private readonly KuzzleApiMock _api;

    public DocumentControllerTest() {
      _api = new KuzzleApiMock();
      _documentController = new DocumentController(_api.MockedObject);
    }

    public static IEnumerable<object[]> GenerateDocumentOptions() {
      yield return new object[] { null };
      yield return new object[] { new DocumentOptions() };
      yield return new object[] {
        new DocumentOptions { WaitForRefresh = true, RetryOnConflict = 42 }
      };
    }

    public static IEnumerable<object[]> GenerateIdAndDocumentOptions() {
      yield return new object[] { null, null };
      yield return new object[] {
        null,
        new DocumentOptions()
      };
      yield return new object[] {
        "foo_id",
        null
      };
      yield return new object[] {
        "foo_id",
        new DocumentOptions{}
      };
      yield return new object[] {
        "foo_id",
        new DocumentOptions{ WaitForRefresh = true, RetryOnConflict = 42}
      };
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateSearchFilters),
      MemberType = typeof(MockGenerators))
    ]
    public async void CountAsyncTest(JObject filters) {
      _api.SetResult(@"{result: {count: 42} }");
      int result = await _documentController.CountAsync("foo", "bar", filters);

      _api.Verify(new JObject {
        { "controller", "document" },
        { "action", "count" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", filters }
      });

      Assert.Equal(42, result);
    }

    [Theory]
    [MemberData(nameof(GenerateIdAndDocumentOptions))]
    public async void CreateAsyncTest(string id, DocumentOptions opts) {
      _api.SetResult("{result: {foo: 123}}");

      var content = new JObject { { "foo", "bar" } };

      JObject result = await _documentController.CreateAsync(
        "foo", "bar", content, id, opts);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "create" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content },
        { "_id", id }
      };

      if (opts != null) {
        expected.Merge(opts.ToJson());
      }

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [MemberData(nameof(GenerateIdAndDocumentOptions))]
    public async void CreateOrReplaceAsyncTest(
      string id, DocumentOptions opts
    ) {
      _api.SetResult("{result: {foo: 123}}");

      var content = new JObject { { "foo", "bar" } };

      JObject result = await _documentController.CreateOrReplaceAsync(
        "foo", "bar", content, id, opts);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "createOrReplace" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content },
        { "_id", id }
      };

      if (opts != null) {
        expected.Merge(opts.ToJson());
      }

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [MemberData(nameof(GenerateIdAndDocumentOptions))]
    public async void DeleteAsyncTest(string id, DocumentOptions opts) {
      _api.SetResult("{result: {foo: 123}}");

      await _documentController.DeleteAsync("foo", "bar", id, opts);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "delete" },
        { "index", "foo" },
        { "collection", "bar" },
        { "_id", id }
      };

      if (opts != null) {
        expected.Merge(opts.ToJson());
      }

      _api.Verify(expected);
    }
  }
}

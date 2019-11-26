using System.Collections.Generic;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.DataObjects;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.API.Controllers {
  public static class DocumentControllerGenerators {
    public static IEnumerable<object[]> GenerateCreateOpts() {
      yield return new object[] { null, false };
      yield return new object[] { null, true };
      yield return new object[] { "fooid", false };
      yield return new object[] { "fooid", true };
    }

    public static IEnumerable<object[]> GenerateUpdateOpts() {
      yield return new object[] { true, null };
      yield return new object[] { false, null };
      yield return new object[] { true, 42 };
      yield return new object[] { false, 42 };
    }
  }

  public class DocumentControllerTest {
    private readonly DocumentController _documentController;
    private readonly KuzzleApiMock _api;

    public DocumentControllerTest() {
      _api = new KuzzleApiMock();
      _documentController = new DocumentController(_api.MockedObject);
    }

    [Theory]
    [
      MemberData(
        nameof(MockGenerators.GenerateSearchFilters),
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
    [
      MemberData(
        nameof(DocumentControllerGenerators.GenerateCreateOpts),
        MemberType = typeof(DocumentControllerGenerators))
    ]
    public async void CreateAsyncTest(string id, bool refresh) {
      _api.SetResult("{result: {foo: 123}}");

      var content = new JObject { { "foo", "bar" } };

      JObject result;

      if (id != null) {
        result = await _documentController.CreateAsync(
          "foo", "bar", content, id: id, waitForRefresh: refresh);
      } else {
        result = await _documentController.CreateAsync(
          "foo", "bar", content, waitForRefresh: refresh);
      }

      var expected = new JObject {
        { "controller", "document" },
        { "action", "create" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content },
        { "_id", id },
        {"waitForRefresh", refresh},
      };

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CreateOrReplaceAsyncTest(bool refresh) {
      _api.SetResult("{result: {foo: 123}}");

      var content = new JObject { { "foo", "bar" } };

      JObject result = await _documentController.CreateOrReplaceAsync(
        index: "foo",
        collection: "bar",
        id: "fooid",
        content: content,
        waitForRefresh: refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "createOrReplace" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content },
        { "_id", "fooid" },
        {"waitForRefresh", refresh},
      };

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void DeleteAsyncTest(bool refresh) {
      _api.SetResult("{result: {foo: 123}}");

      await _documentController.DeleteAsync("foo", "bar", "fooid", refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "delete" },
        { "index", "foo" },
        { "collection", "bar" },
        { "_id", "fooid" },
        {"waitForRefresh", refresh},
      };

      _api.Verify(expected);
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateSearchFilters),
      MemberType = typeof(MockGenerators))
    ]
    public async void DeleteByQueryAsyncTest(JObject filters) {
      _api.SetResult("{ result: { ids: [1, 2, 3] } }");

      JArray result = await _documentController.DeleteByQueryAsync(
        "foo", "bar", filters);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "deleteByQuery" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", filters }
      };

      _api.Verify(expected);
      Assert.Equal(
        new JArray { 1, 2, 3 }, result, new JTokenEqualityComparer());
    }

    [Fact]
    public async void GetAsync() {
      _api.SetResult("{result: {foo: 123}}");

      JObject result = await _documentController.GetAsync("foo", "bar", "id");

      var expected = new JObject {
        { "controller", "document" },
        { "action", "get" },
        { "index", "foo" },
        { "collection", "bar" },
        { "_id", "id" }
      };

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void MCreateAsync(bool refresh) {
      _api.SetResult("{ result: { successes: [1, 2, 3], errors: [] } }");

      var documents = new JArray { "foo", "bar", "baz" };

      JObject result = await _documentController.MCreateAsync(
        "foo",
        "bar",
        documents,
        refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mCreate" },
        { "index", "foo" },
        { "collection", "bar" },
        {"waitForRefresh", refresh},
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("documents", documents);

      _api.Verify(expected);

      Assert.Equal(
        JObject.Parse("{ successes: [1, 2, 3], errors: [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void MCreateOrReplaceAsync(bool refresh) {
      _api.SetResult("{ result: { successes: [1, 2, 3], errors: [] } }");

      var documents = new JArray { "foo", "bar", "baz" };

      JObject result = await _documentController.MCreateOrReplaceAsync(
        "foo",
        "bar",
        documents,
        refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mCreateOrReplace" },
        { "index", "foo" },
        { "collection", "bar" },
        {"waitForRefresh", refresh},
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("documents", documents);

      _api.Verify(expected);

      Assert.Equal(
        JObject.Parse("{ successes: [1, 2, 3], errors: [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void MDeleteAsync(bool refresh) {
      _api.SetResult("{ result: { successes: [1, 2, 3], errors: [] } }");

      var ids = new string[] { "foo", "bar", "baz" };

     JObject result = await _documentController.MDeleteAsync(
        "foo",
        "bar",
        ids,
        refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mDelete" },
        { "index", "foo" },
        { "collection", "bar" },
        {"waitForRefresh", refresh},
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("ids", new JArray(ids));

      _api.Verify(expected);

      Assert.Equal(
        JObject.Parse("{ successes: [1, 2, 3], errors: [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void MGetAsyncTest() {
      _api.SetResult("{ result: { successes: ['foo', 'bar', 'baz'], errors: [] } }");

      var ids = new JArray { "foo", "bar", "baz" };

      JObject result = await _documentController.MGetAsync(
        "foo", "bar", ids);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mGet" },
        { "index", "foo" },
        { "collection", "bar" }
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("ids", ids);

      _api.Verify(expected);
      Assert.Equal(
        JObject.Parse("{ successes: ['foo', 'bar', 'baz'], errors: [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void MReplaceAsync(bool refresh) {
      _api.SetResult("{ result: { successes: [1, 2, 3], errors: [] } }");

      var documents = new JArray { "foo", "bar", "baz" };

      JObject result = await _documentController.MReplaceAsync(
        "foo",
        "bar",
        documents,
        refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mReplace" },
        { "index", "foo" },
        { "collection", "bar" },
        {"waitForRefresh", refresh},
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("documents", documents);

      _api.Verify(expected);

      Assert.Equal(
        JObject.Parse("{ successes: [1, 2, 3], errors: [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [
      MemberData(
        nameof(DocumentControllerGenerators.GenerateUpdateOpts),
        MemberType = typeof(DocumentControllerGenerators))
    ]
    public async void MUpdateAsyncTest(bool refresh, int? retries) {
      _api.SetResult("{ result: { successes: [1, 2, 3], errors: [] } }");

      var documents = new JArray { "foo", "bar", "baz" };

      JObject result;

      if (retries == null) {
        result = await _documentController.MUpdateAsync(
          "foo",
          "bar",
          documents,
          waitForRefresh: refresh);
      } else {
        result = await _documentController.MUpdateAsync(
          "foo",
          "bar",
          documents,
          waitForRefresh: refresh,
          retryOnConflict: (int)retries);
      }

      var expected = new JObject {
        { "controller", "document" },
        { "action", "mUpdate" },
        { "index", "foo" },
        { "collection", "bar" },
        {"waitForRefresh", refresh},
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("documents", documents);

      expected.Add("retryOnConflict", retries ?? 0);

      _api.Verify(expected);
      Assert.Equal(
        JObject.Parse("{ 'successes': [1, 2, 3], 'errors': [] }"),
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void ReplaceAsyncTest(bool refresh) {
      _api.SetResult("{result: {foo: 123}}");

      var content = new JObject { { "foo", "bar" } };

      JObject result = await _documentController.ReplaceAsync(
        index: "foo",
        collection: "bar",
        id: "fooid",
        content: content,
        waitForRefresh: refresh);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "replace" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content },
        { "_id", "fooid" },
        {"waitForRefresh", refresh},
      };

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } }, result, new JTokenEqualityComparer());
    }

    [Theory]
    [
      MemberData(
        nameof(MockGenerators.GenerateSearchOptions),
        MemberType = typeof(MockGenerators))
    ]
    public async void SearchAsyncTest(SearchOptions opts) {
      _api.SetResult(@"{
        result: {
          scrollId: 'scrollId',
          hits: ['foo', 'bar', 'baz'],
          total: 42,
          aggregations: { foo: 'bar' }
        }
      }");

      var filters = JObject.Parse(@"{
        query: { match_all: {} },
        sort: [1, 2, 3]
      }");

      SearchResult result = await _documentController.SearchAsync(
        "foo", "bar", filters, opts);

      var expected = new JObject {
        { "controller", "document" },
        { "action", "search" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", filters }
      };

      if (opts != null) {
        expected.Merge(JObject.FromObject(opts));
      }

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", "bar" } },
        result.Aggregations,
        new JTokenEqualityComparer());

      Assert.Equal(
        new JArray { "foo", "bar", "baz" },
        result.Hits,
        new JTokenEqualityComparer());

      Assert.Equal(42, result.Total);
      Assert.Equal(3, result.Fetched);
      Assert.Equal("scrollId", result.ScrollId);
    }

    [Theory]
    [
      MemberData(
        nameof(DocumentControllerGenerators.GenerateUpdateOpts),
        MemberType = typeof(DocumentControllerGenerators))
    ]
    public async void UpdateAsyncTest(bool refresh, int? retries) {
      _api.SetResult("{ result: { foo: 123 } }");

      var changes = new JObject { { "foo", "bar" } };
      JObject result;

      if (retries == null) {
        result = await _documentController.UpdateAsync(
          "foo", "bar", "id", changes, waitForRefresh: refresh);
      } else {
        result = await _documentController.UpdateAsync(
          "foo",
          "bar",
          "id",
          changes,
          waitForRefresh: refresh,
          retryOnConflict: (int)retries);
      }

      var expected = new JObject {
        { "controller", "document" },
        { "action", "update" },
        { "index", "foo" },
        { "collection", "bar" },
        { "_id", "id" },
        { "body", changes },
        {"waitForRefresh", refresh},
      };

      expected.Add("retryOnConflict", retries ?? 0);

      _api.Verify(expected);

      Assert.Equal(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void ValidateAsyncTest() {
      _api.SetResult("{ result: { valid: true } }");

      var content = new JObject { { "foo", "bar" } };
      bool valid = await _documentController.ValidateAsync(
        "foo", "bar", content);

      _api.Verify(new JObject {
        { "controller", "document" },
        { "action", "validate" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", content }
      });

      Assert.True(valid);
    }
  }
}

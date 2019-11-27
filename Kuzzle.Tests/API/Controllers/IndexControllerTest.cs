using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.API.Controllers {
  public class IndexControllerTest {
    private readonly IndexController _indexController;
    private readonly KuzzleApiMock _api;

    public IndexControllerTest() {
      _api = new KuzzleApiMock();
      _indexController = new IndexController(_api.MockedObject);
    }

    [Fact]
    public async void CreateAsyncTest() {
      _api.SetResult(@"
        {
          result: {
            acknowledged: true,
            shards_acknowledged: true
          }
        }
      ");

      await _indexController.CreateAsync("foo");

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "create" },
        { "index", "foo" }
      });
    }

    [Fact]
    public async void DeleteAsyncTest() {
      _api.SetResult(@"
        {
          result: {
            acknowledged: true
          }
        }
      ");

      await _indexController.DeleteAsync("foo");

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "delete" },
        { "index", "foo" }
      });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void ExistsAsyncTest(bool result) {
      _api.SetResult(new JObject { { "result" , result } } );

      Assert.Equal(
        result,
        await _indexController.ExistsAsync("foo")
      );

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "exists" },
        { "index", "foo" }
      });
    }

    [Fact]
    public async void ListAsyncTest() {
      var indexes = new JArray { "foo", "bar", "zoo" };
      _api.SetResult(new JObject {
        { "result" , new JObject { { "indexes", indexes } } }
      });

      Assert.Equal(
        indexes,
        await _indexController.ListAsync(),
        new JTokenEqualityComparer()
      );

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "list" },
      });
    }

    [Fact]
    public async void MDeleteAsyncTest() {
      var indexes = new JArray { "foo", "bar", "zoo" };
      _api.SetResult(new JObject {
        { "result" , new JObject { { "deleted", indexes } } }
      });

      Assert.Equal(
        indexes,
        await _indexController.MDeleteAsync(indexes),
        new JTokenEqualityComparer()
      );

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "mDelete" },
        { "body", new JObject { { "indexes", indexes } } }
      });
    }
  }
}

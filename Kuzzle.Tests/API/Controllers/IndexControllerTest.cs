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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void GetAutoRefreshAsyncTest(bool result) {
      _api.SetResult(new JObject { { "result" , result } } );

      Assert.Equal(
        result, 
        await _indexController.GetAutoRefreshAsync("foo")
      );

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "getAutoRefresh" },
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

    [Fact]
    public async void RefreshAsyncTest() {
      _api.SetResult(@"
        {
          result: {
            _shards: {
              failed: 0,
              successful: 5,
              total: 10
            }
          }
        }
      ");

      Assert.Equal(new JObject {
        { "failed", 0 },
        { "successful", 5 },
        { "total", 10 }
      }, await _indexController.RefreshAsync("foo"));

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "refresh" },
        { "index", "foo" }
      });
    }

    [Fact]
    public async void RefreshInternalAsyncTest() {
      _api.SetResult(@"
        {
          result: {
            acknowledged: true 
          }
        }
      ");

      await _indexController.RefreshInternalAsync();

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "refreshInternal" },
      });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void SetAutoRefreshAsyncTest(bool autoRefresh) {
      _api.SetResult(new JObject { 
        { "result" , new JObject { { "response", autoRefresh } } } 
      });

      await _indexController.SetAutoRefreshAsync("foo", autoRefresh);

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "setAutoRefresh" }, 
        { "index", "foo" },
        { "body", new JObject { { "autoRefresh", autoRefresh } } }
      });
    }
  }
}

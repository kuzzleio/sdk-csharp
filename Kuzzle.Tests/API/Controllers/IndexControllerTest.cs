using System;
using System.Collections.Generic;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.DataObjects;
using KuzzleSdk.API.Options;
using Newtonsoft.Json;
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
      var expected = JObject.Parse(@"
      {
				acknowledged: true, 
				shards_acknowledged: true
			} ");

      _api.SetResult(new JObject { { "result", expected } });

      JObject result = await _indexController.CreateAsync("foo");
      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "create" },
        { "index", "foo" }
      });

			Assert.Equal(expected, result);
		}

    [Fact]
    public async void DeleteAsyncTest() {
      _api.SetResult(@"{result: {acknowledged: true} }");
      await _indexController.DeleteAsync("foo");

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "delete" },
        { "index", "foo" }
      });
		}

    [Fact]
    public async void ExistsAsyncTest() {
      _api.SetResult(@"{result: true}");
      bool result = await _indexController.ExistsAsync("foo");

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "exists" },
        { "index", "foo" }
      });
			Assert.True(result);
		}

    [Fact]
    public async void GetAutoRefreshAsyncTest() {
      _api.SetResult(@"{result: true}");
      bool result = await _indexController.GetAutoRefreshAsync("foo");

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "getAutoRefresh" },
        { "index", "foo" }
      });
			Assert.True(result);
		}

    [Fact]
    public async void ListAsyncTest() {
      _api.SetResult(@"{result: { indexes: ['foo', 'bar', 'zoo'] } }");
      JArray result = await _indexController.ListAsync();

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "list" },
      });

      Assert.Equal(
        new JArray { "foo", "bar", "zoo" },
        result,
        new JTokenEqualityComparer());
		}

    [Fact]
    public async void MDeleteAsyncTest() {
			var indexes = new JArray { "foo", "bar", "zoo" };
			_api.SetResult(@"{result: { indexes:  ['foo', 'bar', 'zoo'] } }");

      JArray result = await _indexController.MDeleteAsync(indexes);

      var expected = new JObject {
        { "controller", "index" },
        { "action", "mDelete" },
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("indexes", indexes);

      _api.Verify(expected);

      Assert.Equal(
        indexes,
        result,
        new JTokenEqualityComparer());
		}

    [Fact]
    public async void RefreshAsyncTest() {
      var expected = JObject.Parse(@"
      {
				_shards: {
					failed: 0,
					succressful: 5,
					total: 10
				}
			} ");

      _api.SetResult(new JObject { { "result", expected } });

      JObject result = await _indexController.RefreshAsync("foo");
      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "refresh" },
        { "index", "foo" }
      });

			Assert.Equal(expected, result);
		}

    [Fact]
    public async void RefreshInternalAsyncTest() {
      _api.SetResult(@"{result: { acknowledged: true }}");
      bool result = await _indexController.RefreshInternalAsync();

      _api.Verify(new JObject {
        { "controller", "index" },
        { "action", "refreshInternal" },
      });
			Assert.True(result);
		}

    [Fact]
    public async void SetAutoRefreshAsyncTest() {
      _api.SetResult(@"{result: { response: true } }");
      var expected = new JObject {
        { "controller", "index" },
        { "action", "setAutoRefresh" },
        { "index", "foo" }
      };

      expected.Add("body", new JObject());
      ((JObject)expected["body"]).Add("autoRefresh", true);

      bool result = await _indexController.SetAutoRefreshAsync("foo", true);

      _api.Verify(expected);
			Assert.True(result);
		}
  }
}

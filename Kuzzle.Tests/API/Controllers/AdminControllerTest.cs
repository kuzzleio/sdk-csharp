using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Utils;

namespace Kuzzle.Tests.API.Controllers {
  public class AdminControllerTest {
    private readonly AdminController _adminController;
    private readonly KuzzleApiMock _api;

    public AdminControllerTest() {
      _api = new KuzzleApiMock();
      _adminController = new AdminController(_api.MockedObject);
    }


    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void DumpAsyncTestSuccess(bool value) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.DumpAsync();

      _api.Verify(new JObject {
        {"controller", "admin"},
        {"action", "dump"}
      });

    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async void LoadFixturesAsyncTestSuccess(bool value, bool waitForRefresh) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.LoadFixturesAsync(
        JObject.Parse(@"{foo: [{create: {_id: 'bar'}}, {field: 'value', field2: true}]}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"body", new JObject {
          {"index-name", new JObject{
            {"foo", new JArray {
              new JObject{{"create", new JObject {{ "_id", "bar" }} } },
              new JObject{{"field", "value"}, {"field2", true} }
            }}
          }}
        }}
      };

      QueryUtils.HandleRefreshOption(verifyQuery, waitForRefresh);

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async void LoadMappingsAsyncTestSuccess(bool value, bool waitForRefresh) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.LoadMappingsAsync(
        JObject.Parse(@"{foo: {properties: {field1: {}, field2: {}}}}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadMappings"},
        {"body", new JObject {
          {"index-name", new JObject{
            {"foo", new JObject {
              {"properties", new JObject{
                {"field1", new JObject{ }},
                {"field2", new JObject{ }}
              }}
            }}
          }}
        }}
      };

      QueryUtils.HandleRefreshOption(verifyQuery, waitForRefresh);

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async void LoadSecuritiesAsyncTestSuccess(bool value, bool waitForRefresh) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.LoadSecuritiesAsync(
        JObject.Parse(@"{roles: {foobar: {foo: 'bar', bar: 'foo'}}}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadSecurities"},
        {"body", new JObject {
          {"roles", new JObject{
            {"foobar", new JObject {
              {"foo", "bar"},
              {"bar", "foo"}
            }}
          }}
        }}
      };

      QueryUtils.HandleRefreshOption(verifyQuery, waitForRefresh);

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void ResetCacheAsyncTestSuccess(bool value) {
      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.ResetCacheAsync("foobar");

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetCache"},
        {"database", "foobar"}
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void ResetDatabaseAsyncTestSuccess(bool value) {
      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.ResetDatabaseAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetDatabase"},
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void ResetKuzzleDataAsyncTestSuccess(bool value) {
      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.ResetKuzzleDataAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetKuzzleData"},
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void ResetSecurityAsyncTestSuccess(bool value) {
      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      await _adminController.ResetSecurityAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetSecurity"},
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void ShutdownAsyncTestSuccess(bool value) {
      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

     await _adminController.ShutdownAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "shutdown"},
      };

      _api.Verify(verifyQuery);

    }

  }
}

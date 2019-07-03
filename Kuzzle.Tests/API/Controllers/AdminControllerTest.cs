using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;
using Moq;

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

      bool result = await _adminController.DumpAsync();

      _api.Verify(new JObject {
        {"controller", "admin"},
        {"action", "dump"}
      });

      Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void LoadFixturesAsyncTestSuccess(bool value) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      bool result = await _adminController.LoadFixturesAsync(
        JObject.Parse(@"{foo: [{create: {_id: 'bar'}}, {field: 'value', field2: true}]}"),
        "wait_for");

      _api.Verify(new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"refresh", "wait_for"},
        {"body", new JObject {
          {"index-name", new JObject{
            {"foo", new JArray {
              new JObject{{"create", new JObject {{ "_id", "bar" }} } },
              new JObject{{"field", "value"}, {"field2", true} }
            }}
          }}
        }}
      });
        
      Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void LoadMappingsAsyncTestSuccess(bool value) {

      _api.SetResult(new JObject {
          {"result", new JObject {{"acknowledge", value}}
        }
      });

      bool result = await _adminController.LoadFixturesAsync(
        JObject.Parse(@"{foo: {properties: {field1: {}, field2: {}}}}"),
        "wait_for");

      _api.Verify(new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"refresh", "wait_for"},
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
      });

      Assert.Equal(value, result);
    }

  }
}

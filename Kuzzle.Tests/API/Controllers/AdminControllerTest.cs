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
  }
}

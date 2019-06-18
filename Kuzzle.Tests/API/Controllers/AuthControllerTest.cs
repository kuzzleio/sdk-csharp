using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;

namespace Kuzzle.Tests.API.Controllers {
  public class AuthControllerTest {
    private readonly AuthController _authController;
    private readonly KuzzleApiMock _api;

    public AuthControllerTest() {
      _api = new KuzzleApiMock();
      _authController = new AuthController(_api.MockedObject);
    }

    [Fact]
    public async void CheckTokenAsyncTestSuccess() {
      string token = "foobar";

      _api.SetResult(@"{result: {foo: 123}}");
      _api.Mock.SetupProperty(a => a.Jwt, "foo");

      JObject result = await _authController.CheckTokenAsync(token);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "checkToken" },
        { "body", new JObject{ { "token", token } } }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());

      _api.Mock.VerifySet(a => a.Jwt = null);
      _api.Mock.VerifySet(a => a.Jwt = "foo");
    }

    [Fact]
    public async void CheckTokenAsyncTestFailure() {
      string token = "foobar";

      _api.SetError();
      _api.Mock.SetupProperty(a => a.Jwt, "foo");

      await Assert.ThrowsAsync<ApiErrorException>(
        () => _authController.CheckTokenAsync(token));

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "checkToken" },
        { "body", new JObject{ { "token", token } } }
      });

      _api.Mock.VerifySet(a => a.Jwt = null);
      _api.Mock.VerifySet(a => a.Jwt = "foo");
    }
  }
}
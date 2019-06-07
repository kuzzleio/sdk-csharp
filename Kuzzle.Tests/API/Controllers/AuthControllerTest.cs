using Xunit;
using Moq;
using KuzzleSdk;
using KuzzleSdk.API.Controllers;

namespace Kuzzle.Tests.API.Controllers {
  public class AuthControllerTest {
    private readonly AuthController _authController;
    private readonly Mock _apiMock;

    public AuthControllerTest() {
      _apiMock = new Mock<IKuzzleApi>();
      _authController = new AuthController((IKuzzleApi)_apiMock.Object);
    }
  }
}

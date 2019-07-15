using System;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using Moq;

namespace Kuzzle.Tests.API {
  public class TestableKuzzle : IKuzzle {

    public Mock<IRealtimeController> mockedRealtimeController;
    public Mock<IAuthController> mockedAuthController;
    public Mock<IKuzzle> mockedKuzzle;

    public TestableKuzzle() {
      mockedRealtimeController = new Mock<IRealtimeController>();
      mockedAuthController = new Mock<IAuthController>();
      mockedKuzzle = new Mock<IKuzzle>();
    }

    public string AuthenticationToken { get; set; } = "";

    public IKuzzle GetKuzzle() {
      return mockedKuzzle.Object;
    }

    public IAuthController GetAuth() {
      return mockedAuthController.Object;
    }

    public IRealtimeController GetRealtime() {
      return mockedRealtimeController.Object;
    }

    public TaskCompletionSource<Response> GetRequestById(string requestId) {
      return mockedKuzzle.Object.GetRequestById(requestId);
    }
  }
}


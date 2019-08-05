using System;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.EventHandler;
using Moq;

namespace Kuzzle.Tests.API {
  internal class TestableKuzzle : IKuzzle {

    internal Mock<IRealtimeController> mockedRealtimeController;
    internal Mock<IAuthController> mockedAuthController;
    internal Mock<IKuzzle> mockedKuzzle;
    internal Mock<AbstractKuzzleEventHandler> mockedKuzzleEventHandler;

    public TestableKuzzle() {
      mockedRealtimeController = new Mock<IRealtimeController>();
      mockedAuthController = new Mock<IAuthController>();
      mockedKuzzle = new Mock<IKuzzle>();
      mockedKuzzleEventHandler = new Mock<AbstractKuzzleEventHandler>();
    }

    public string AuthenticationToken { get; set; } = "";

    public AbstractKuzzleEventHandler GetEventHandler() {
      return mockedKuzzleEventHandler.Object;
    }

    public IKuzzle GetKuzzle() {
      return mockedKuzzle.Object;
    }

    IAuthController IKuzzle.GetAuth() {
      return mockedAuthController.Object;
    }

    IRealtimeController IKuzzle.GetRealtime() {
      return mockedRealtimeController.Object;
    }

    TaskCompletionSource<Response> IKuzzle.GetRequestById(string requestId) {
      return mockedKuzzle.Object.GetRequestById(requestId);
    }
  }
}


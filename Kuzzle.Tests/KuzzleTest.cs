using Xunit;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;
using Kuzzle.Tests.Protocol;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using System;

namespace Kuzzle.Tests {
  public class KuzzleTest {
    private readonly KuzzleSdk.Kuzzle _kuzzle;
    private readonly ProtocolMock _protocol;

    public KuzzleTest() {
      _protocol = new ProtocolMock();
      _kuzzle = new KuzzleSdk.Kuzzle(_protocol.MockedObject);
    }

    [Fact]
    public void DispatchTokenExpiredTest() {
      bool eventDispatched = false;

      _kuzzle.TokenExpired += delegate() {
        eventDispatched = true;
      };

      _kuzzle.DispatchTokenExpired();

      Assert.Null(_kuzzle.AuthenticationToken);
      Assert.True(eventDispatched);
    }

    [Fact]
    public void KuzzleConstructorTest() {
      Assert.NotNull(_kuzzle.Auth);
      Assert.NotNull(_kuzzle.Collection);
      Assert.NotNull(_kuzzle.Document);
      Assert.NotNull(_kuzzle.Realtime);
      Assert.NotNull(_kuzzle.Server);
      Assert.NotNull(_kuzzle.Version);
      Assert.NotNull(_kuzzle.InstanceId);
    }
  }
}
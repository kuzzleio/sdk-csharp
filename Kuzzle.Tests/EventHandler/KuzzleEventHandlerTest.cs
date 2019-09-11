using System;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.EventHandler.Events.SubscriptionEvents;
using KuzzleSdk.Protocol;
using Moq;
using Xunit;

namespace Kuzzle.Tests.EventHandler {
  public class KuzzleEventHandlerTest {
    private readonly KuzzleSdk.Kuzzle _kuzzle;
    private readonly Mock<AbstractProtocol> _protocol;

    public KuzzleEventHandlerTest() {
      _protocol = new Mock<AbstractProtocol>();
      _kuzzle = new KuzzleSdk.Kuzzle(_protocol.Object);
    }

    [Fact]
    public void DispatchTokenExpiredTest() {
      _kuzzle.AuthenticationToken = "token";
      bool eventDispatched = false;
      _kuzzle.EventHandler.TokenExpired += delegate () {
        eventDispatched = true;
      };

      _kuzzle.EventHandler.DispatchTokenExpired();

      Assert.Null(_kuzzle.AuthenticationToken);
      Assert.True(eventDispatched);
    }

    [Fact]
    public void DispatchQueueRecoveredTest() {
      bool eventDispatched = false;
      _kuzzle.EventHandler.QueueRecovered += delegate () {
        eventDispatched = true;
      };

      _kuzzle.EventHandler.DispatchQueueRecovered();

      Assert.True(eventDispatched);
    }

    [Fact]
    public void DispatchUserLoggedInTest() {
      bool eventDispatched = false;
      string name = "";
      _kuzzle.EventHandler.UserLoggedIn += delegate (object sender, UserLoggedInEvent e) {
        eventDispatched = true;
        name = e.Kuid;
      };

      _kuzzle.EventHandler.DispatchUserLoggedIn("foobar");

      Assert.Equal("foobar", name);
      Assert.True(eventDispatched);
    }

    [Fact]
    public void DispatchReconnectedTest() {
      bool eventDispatched = false;
      _kuzzle.EventHandler.Reconnected += delegate () {
        eventDispatched = true;
      };

      _kuzzle.EventHandler.DispatchReconnected();

      Assert.True(eventDispatched);
    }

    [Fact]
    public void DispatchSubscriptionTest() {
      bool eventDispatched = false;
      _kuzzle.EventHandler.Subscription += delegate (object sender, SubscriptionEvent e) {
        eventDispatched = true;
      };

      _kuzzle.EventHandler.DispatchSubscription(new SubscriptionClearEvent());

      Assert.True(eventDispatched);
    }


  }
}

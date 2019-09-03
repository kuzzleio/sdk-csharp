using System;
using System.ComponentModel;
using KuzzleSdk.API;
using KuzzleSdk.EventHandler.Events;

namespace KuzzleSdk.EventHandler {
  public sealed class KuzzleEventHandler : AbstractKuzzleEventHandler {

    private IKuzzleApi kuzzle;

    public KuzzleEventHandler(IKuzzleApi kuzzle) {
      this.kuzzle = kuzzle;
    }

    private EventHandlerList eventHandlerList = new EventHandlerList();

    private static readonly object subscriptionEventKey = new object();       // Internal
    private static readonly object userLoggedInEventKey = new object();       // Internal
    private static readonly object userLoggedOutEventKey = new object();      // Internal
    private static readonly object reconnectedEventKey = new object();        // Public
    private static readonly object queueRecoveredEventKey = new object();     // Public
    private static readonly object unhandledResponseEventKey = new object();  // Public
    private static readonly object tokenExpiredEventKey = new object();       // Public

    /// <summary>
    /// Occurs when we have to [add, remove, clear] subscriptions :
    /// </summary>
    public override event EventHandler<SubscriptionEvent> Subscription {
      add {
        eventHandlerList.AddHandler(subscriptionEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(subscriptionEventKey, value);
      }
    }

    internal override void DispatchSubscription(SubscriptionEvent subscriptionData) {
      EventHandler<SubscriptionEvent> subscriptionEvent =
      (EventHandler<SubscriptionEvent>)eventHandlerList[subscriptionEventKey];

      subscriptionEvent?.Invoke(this, subscriptionData);
    }

    /// <summary>
    /// Occurs when a user has logged in
    /// </summary>
    public override event EventHandler<UserLoggedInEvent> UserLoggedIn {
      add {
        eventHandlerList.AddHandler(userLoggedInEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(userLoggedInEventKey, value);
      }
    }

    internal override void DispatchUserLoggedIn(string kuid) {
      EventHandler<UserLoggedInEvent> userLoggedIn =
      (EventHandler<UserLoggedInEvent>)eventHandlerList[userLoggedInEventKey];

      userLoggedIn?.Invoke(this, new UserLoggedInEvent(kuid));
    }

    /// <summary>
    /// Occurs when the successfuly reconnected to the network
    /// </summary>
    public override event Action Reconnected {
      add {
        eventHandlerList.AddHandler(reconnectedEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(reconnectedEventKey, value);
      }
    }

    internal override void DispatchReconnected() {
      Action queueRecovered = (Action)eventHandlerList[reconnectedEventKey];
      queueRecovered?.Invoke();
    }

    /// <summary>
    /// Occurs when the offline queue of query has been successfuly recovered
    /// </summary>
    public override event Action QueueRecovered {
      add {
        eventHandlerList.AddHandler(queueRecoveredEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(queueRecoveredEventKey, value);
      }
    }

    internal override void DispatchQueueRecovered() {
      Action queueRecovered = (Action)eventHandlerList[queueRecoveredEventKey];
      queueRecovered?.Invoke();
    }

    /// <summary>
    /// Occurs when an unhandled response is received.
    /// </summary>
    public override event EventHandler<Response> UnhandledResponse {
      add {
        eventHandlerList.AddHandler(unhandledResponseEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(unhandledResponseEventKey, value);
      }
    }

    internal override void DispatchUnhandledResponse(Response response) {
      EventHandler<Response> unhandledResponse =
      (EventHandler<Response>)eventHandlerList[unhandledResponseEventKey];

      unhandledResponse?.Invoke(this, response);
    }

    /// <summary>
    /// Token expiration event 
    /// </summary>
    public override event Action TokenExpired {
      add {
        eventHandlerList.AddHandler(tokenExpiredEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(tokenExpiredEventKey, value);
      }
    }

    internal override void DispatchTokenExpired() {
      kuzzle.AuthenticationToken = null;
      Action tokenExpired = (Action)eventHandlerList[tokenExpiredEventKey];
      tokenExpired?.Invoke();
    }

    public override event Action UserLoggedOut {
      add {
        eventHandlerList.AddHandler(userLoggedOutEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(userLoggedOutEventKey, value);
      }
    }

    internal override void DispatchUserLoggedOut() {
      Action userLoggedOut = (Action)eventHandlerList[userLoggedOutEventKey];
      userLoggedOut?.Invoke();
    }
  }
}

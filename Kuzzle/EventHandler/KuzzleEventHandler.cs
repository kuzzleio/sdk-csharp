using System;
using System.ComponentModel;
using KuzzleSdk.API;
using KuzzleSdk.EventHandler.Events;

namespace KuzzleSdk.EventHandler {
  public sealed class KuzzleEventHandler : IKuzzleEventHandler {

    private Kuzzle kuzzle;

    public KuzzleEventHandler(Kuzzle kuzzle) {
      this.kuzzle = kuzzle;
    }

    private EventHandlerList eventHandlerList = new EventHandlerList();

    private static readonly object subscriptionEventKey = new object();       // Internal
    private static readonly object userLoggedInEventKey = new object();       // Internal
    private static readonly object reconnectedEventKey = new object();        // Public
    private static readonly object queueRecoveredEventKey = new object();     // Public
    private static readonly object unhandledResponseEventKey = new object();  // Public
    private static readonly object tokenExpiredEventKey = new object();       // Public

    /// <summary>
    /// Occurs when subscription has to be :
    /// - Added
    /// - Removed
    /// - Cleared
    /// </summary>
    public event EventHandler<SubscriptionEvent> Subscription {
      add {
        eventHandlerList.AddHandler(subscriptionEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(subscriptionEventKey, value);
      }
    }

    public void DispatchSubscription(SubscriptionEvent subscriptionData) {
      EventHandler<SubscriptionEvent> subscriptionEvent = (EventHandler<SubscriptionEvent>)eventHandlerList[subscriptionEventKey];
      subscriptionEvent?.Invoke(this, subscriptionData);
    }

    /// <summary>
    /// Occurs when a user has logged in
    /// </summary>
    public event EventHandler<UserLoggedInEvent> UserLoggedIn {
      add {
        eventHandlerList.AddHandler(userLoggedInEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(userLoggedInEventKey, value);
      }
    }

    public void DispatchUserLoggedIn(string username) {
      EventHandler<UserLoggedInEvent> userLoggedIn = (EventHandler<UserLoggedInEvent>)eventHandlerList[userLoggedInEventKey];
      userLoggedIn?.Invoke(this, new UserLoggedInEvent(username));
    }

    /// <summary>
    /// Occurs when the successfuly reconnected to the network
    /// </summary>
    public event Action Reconnected {
      add {
        eventHandlerList.AddHandler(reconnectedEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(reconnectedEventKey, value);
      }
    }

    public void DispatchReconnected() {
      Action queueRecovered = (Action)eventHandlerList[reconnectedEventKey];
      queueRecovered?.Invoke();
    }

    /// <summary>
    /// Occurs when the offline queue of query has been successfuly recovered
    /// </summary>
    public event Action QueueRecovered {
      add {
        eventHandlerList.AddHandler(queueRecoveredEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(queueRecoveredEventKey, value);
      }
    }

    public void DispatchQueueRecovered() {
      Action queueRecovered = (Action)eventHandlerList[queueRecoveredEventKey];
      queueRecovered?.Invoke();
    }

    /// <summary>
    /// Occurs when an unhandled response is received.
    /// </summary>
    public event EventHandler<Response> UnhandledResponse {
      add {
        eventHandlerList.AddHandler(unhandledResponseEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(unhandledResponseEventKey, value);
      }
    }

    public void DispatchUnhandledResponse(Response response) {
      EventHandler<Response> unhandledResponse = (EventHandler<Response>)eventHandlerList[unhandledResponseEventKey];
      unhandledResponse?.Invoke(this, response);
    }

    /// <summary>
    /// Token expiration event 
    /// </summary>
    public event Action TokenExpired {
      add {
        eventHandlerList.AddHandler(tokenExpiredEventKey, value);
      }

      remove {
        eventHandlerList.RemoveHandler(tokenExpiredEventKey, value);
      }
    }

    public void DispatchTokenExpired() {
      kuzzle.AuthenticationToken = null;
      Action tokenExpired = (Action)eventHandlerList[tokenExpiredEventKey];
      tokenExpired?.Invoke();
    }
  }
}

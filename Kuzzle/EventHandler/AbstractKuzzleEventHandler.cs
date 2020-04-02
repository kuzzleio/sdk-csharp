using System;
using KuzzleSdk.API;
using KuzzleSdk.EventHandler.Events;

namespace KuzzleSdk.EventHandler {
  /// <summary>
  /// Abstract class describing this SDK event handler
  /// </summary>
  public abstract class AbstractKuzzleEventHandler {
    /// <summary>
    /// Events occuring on realtime subscriptions
    /// </summary>
    public abstract event EventHandler<SubscriptionEvent> Subscription;
    /// <summary>
    /// Events occuring on a successful login
    /// </summary>
    public abstract event EventHandler<UserLoggedInEvent> UserLoggedIn;
    /// <summary>
    /// Events occuring on a successful logout
    /// </summary>
    public abstract event Action UserLoggedOut;
    /// <summary>
    /// Events occuring whenever the SDK reconnects after a connection loss
    /// </summary>
    public abstract event Action Reconnected;
    /// <summary>
    /// Events occuring when queued items during a connection loss have all
    /// been replayed
    /// </summary>
    public abstract event Action QueueRecovered;
    /// <summary>
    /// Events occuring on an unknown query response received from Kuzzle
    /// </summary>
    public abstract event EventHandler<Response> UnhandledResponse;
    /// <summary>
    /// Events occuring when the authentication token has expired
    /// </summary>
    public abstract event Action TokenExpired;

    internal abstract void DispatchSubscription(SubscriptionEvent subscriptionData);
    internal abstract void DispatchUserLoggedIn(string kuid);
    internal abstract void DispatchUserLoggedOut();
    internal abstract void DispatchReconnected();
    internal abstract void DispatchQueueRecovered();
    internal abstract void DispatchUnhandledResponse(Response response);
    internal abstract void DispatchTokenExpired();
  }
}

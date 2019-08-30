using System;
using KuzzleSdk.API;
using KuzzleSdk.EventHandler.Events;

namespace KuzzleSdk.EventHandler {
  public abstract class AbstractKuzzleEventHandler {
    public abstract event EventHandler<SubscriptionEvent> Subscription;
    public abstract event EventHandler<UserLoggedInEvent> UserLoggedIn;
    public abstract event Action UserLoggedOut;
    public abstract event Action Reconnected;
    public abstract event Action QueueRecovered;
    public abstract event EventHandler<Response> UnhandledResponse;
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

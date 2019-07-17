using System;
using KuzzleSdk.API;
using KuzzleSdk.EventHandler.Events;

namespace KuzzleSdk.EventHandler {
  public interface IKuzzleEventHandler {
    event EventHandler<SubscriptionEvent> Subscription;
    event EventHandler<UserLoggedInEvent> UserLoggedIn;
    event Action Reconnected;
    event Action QueueRecovered;
    event EventHandler<Response> UnhandledResponse;
    event Action TokenExpired;

    void DispatchSubscription(SubscriptionEvent subscriptionData);
    void DispatchUserLoggedIn(string username);
    void DispatchReconnected();
    void DispatchQueueRecovered();
    void DispatchUnhandledResponse(Response response);
    void DispatchTokenExpired();
    }
}

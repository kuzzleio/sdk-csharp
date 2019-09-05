using System;
using KuzzleSdk.Offline.Subscription;

namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  public sealed class SubscriptionAddEvent : SubscriptionEvent {

    public Subscription SubscriptionData { get; private set; }

    public SubscriptionAddEvent(Subscription subscription) : base(SubscriptionAction.Add) {
      SubscriptionData = subscription;
    }
  }
}

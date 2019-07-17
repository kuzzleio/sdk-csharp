using System;
using KuzzleSdk.Offline.Subscription;

namespace KuzzleSdk.EventHandler.Events {
  public enum SubscriptionAction {
    Add,
    Remove,
    Clear,
  }
  public class SubscriptionEvent : EventArgs {

    public SubscriptionAction Action { get; private set; }
    public Subscription Sub { get; private set; }

    public SubscriptionEvent(SubscriptionAction action) {
      Action = action;
    }

    public SubscriptionEvent(SubscriptionAction action, Subscription subscription) {
      Action = action;
      Sub = subscription;
    }

  }
}

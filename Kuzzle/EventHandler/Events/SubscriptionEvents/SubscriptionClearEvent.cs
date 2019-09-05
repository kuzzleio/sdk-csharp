using System;
namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  public sealed class SubscriptionClearEvent : SubscriptionEvent {
    public SubscriptionClearEvent() : base(SubscriptionAction.Clear) {
    }
  }
}

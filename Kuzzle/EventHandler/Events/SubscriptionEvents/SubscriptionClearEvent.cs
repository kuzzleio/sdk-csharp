using System;
namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  /// <summary>
  /// Event triggered when realtime subscriptions are cleared up
  /// </summary>
  public sealed class SubscriptionClearEvent : SubscriptionEvent {
    /// <summary>
    /// Event triggered when realtime subscriptions are cleared up
    /// </summary>
    public SubscriptionClearEvent() : base(SubscriptionAction.Clear) {
    }
  }
}

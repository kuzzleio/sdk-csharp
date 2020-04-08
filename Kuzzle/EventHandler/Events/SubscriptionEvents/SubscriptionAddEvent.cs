using System;
using KuzzleSdk.Offline.Subscription;

namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  /// <summary>
  /// Event triggered on new realtime subscriptions
  /// </summary>
  public sealed class SubscriptionAddEvent : SubscriptionEvent {
    /// <summary>
    /// Realtime subscription information
    /// </summary>
    public Subscription SubscriptionData { get; private set; }

    /// <summary>
    /// Event triggered on new realtime subscriptions
    /// </summary>
    public SubscriptionAddEvent(Subscription subscription)
      : base(SubscriptionAction.Add)
    {
      SubscriptionData = subscription;
    }
  }
}

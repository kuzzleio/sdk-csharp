using System;
using KuzzleSdk.Offline.Subscription;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  /// <summary>
  /// Event triggered on realtime subscriptions removal
  /// </summary>
  public sealed class SubscriptionRemoveEvent : SubscriptionEvent {
    /// <summary>
    /// Predicate returning the removed subscription
    /// </summary>
    public Predicate<Subscription> Predicate { get; private set; }

    /// <summary>
    /// Event triggered on realtime subscriptions removal
    /// </summary>
    public SubscriptionRemoveEvent(Predicate<Subscription> predicate)
      : base(SubscriptionAction.Remove)
    {
      Predicate = predicate;
    }
  }
}

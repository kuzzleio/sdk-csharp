using System;
using KuzzleSdk.Offline.Subscription;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.EventHandler.Events.SubscriptionEvents {
  public sealed class SubscriptionRemoveEvent : SubscriptionEvent {

    public Predicate<Subscription> Predicate { get; private set; }

    public SubscriptionRemoveEvent(Predicate<Subscription> predicate) : base(SubscriptionAction.Remove) {
      Predicate = predicate;
    }
  }
}

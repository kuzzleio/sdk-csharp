using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.EventHandler.Events.SubscriptionEvents;

namespace KuzzleSdk.Offline.Subscription {

  public interface ISubscriptionRecoverer {
    void Add(Subscription subscription);
    int Remove(Predicate<Subscription> predicate);
    void Clear();
    void RenewSubscriptions();
  }

  internal sealed class SubscriptionRecoverer : ISubscriptionRecoverer {

    private IRealtimeController realtimeController;
    private List<Subscription> subscriptions = new List<Subscription>();

    public SubscriptionRecoverer(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.realtimeController = kuzzle.GetRealtime();
      if (kuzzle.GetEventHandler() == null)
        throw new Exception("NULL");
      kuzzle.GetEventHandler().Subscription += OnSubscriptionEvent;
    }

    private void OnSubscriptionEvent(object sender, SubscriptionEvent subscriptionEvent) {
      switch (subscriptionEvent.Action) {
        case SubscriptionAction.Add:
          if (subscriptionEvent is SubscriptionAddEvent) {
            Add(((SubscriptionAddEvent)subscriptionEvent).SubscriptionData);
          }
          break;
        case SubscriptionAction.Remove:
          if (subscriptionEvent is SubscriptionRemoveEvent) {
            Remove(((SubscriptionRemoveEvent)subscriptionEvent).Predicate);
          }
          break;
        case SubscriptionAction.Clear:
          Clear();
          break;
        default: break;
      }
    }

    /// <summary>
    /// Add a subscription.
    /// </summary>
    public void Add(Subscription subscription) {
      lock (subscriptions) {
        subscriptions.Add(subscription);
      }
    }

    public int Count { get { return subscriptions.Count; } }

    /// <summary>
    /// Remove every subscriptions that satisfy the predicate.
    /// </summary>
    public int Remove(Predicate<Subscription> predicate) {
      lock (subscriptions) {
        return subscriptions.RemoveAll(predicate);
      }
    }

    /// <summary>
    /// Clear every subscriptions saved.
    /// </summary>
    public void Clear() {
      lock (subscriptions) {
        subscriptions.Clear();
      }
    }

    /// <summary>
    /// Renew one subscription.
    /// </summary>
    private async Task RenewSubscription(Subscription subscription) {

      string roomId = await realtimeController.SubscribeAndAddToRecoverer(
      subscription.Index,
      subscription.Collection,
      subscription.Filters,
      subscription.Handler,
      subscription.Options,
      false);
      subscription.RoomId = roomId;
    }

    /// <summary>
    /// Renew every saved subscriptions.
    /// </summary>
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public void RenewSubscriptions() {
      lock (subscriptions) {
        for (int i = 0; i < subscriptions.Count; i++) {
          Subscription subscription = subscriptions[i];
          RenewSubscription(subscription);
        }
      }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

  }
}

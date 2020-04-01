﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.EventHandler.Events.SubscriptionEvents;

namespace KuzzleSdk.Offline.Subscription {
  /// <summary>
  /// Handle resubscriptions after a connection loss
  /// </summary>
  public interface ISubscriptionRecoverer {
    /// <summary>
    /// Register a new subscription
    /// </summary>
    void Add(Subscription subscription);

    /// <summary>
    /// Remove a subscription
    /// </summary>
    int Remove(Predicate<Subscription> predicate);

    /// <summary>
    /// Clear all subscription from the recoverer
    /// </summary>
    void Clear();

    /// <summary>
    /// Resubmit all registered subscriptions
    /// </summary>
    void RenewSubscriptions();
  }

  internal sealed class SubscriptionRecoverer : ISubscriptionRecoverer {

    private IRealtimeController realtimeController;
    private List<Subscription> subscriptions = new List<Subscription>();

    public SubscriptionRecoverer(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.realtimeController = kuzzle.GetRealtime();
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

    /// <summary>
    /// Returns how many elements are in the queue.
    /// </summary>
    public int Count { get { return subscriptions.Count; } }

    /// <summary>
    /// Remove every subscription that satisfies the predicate.
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
        false
      );

      subscription.RoomId = roomId;
    }

    /// <summary>
    /// Renew every saved subscriptions.
    /// </summary>
    public void RenewSubscriptions() {
      lock (subscriptions) {
        foreach (Subscription subscription in subscriptions) {
          _ = RenewSubscription(subscription);
        }
      }
    }
  }
}

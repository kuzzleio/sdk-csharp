using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;

namespace KuzzleSdk.Offline.Subscription {

  public interface ISubscriptionRecoverer {
    void Add(Subscription subscription);
    void Remove(Predicate<Subscription> predicate);
    void ClearAllSubscriptions();
    void RenewSubscriptions();
  }

  public class SubscriptionRecoverer : ISubscriptionRecoverer {

    private RealtimeController realtimeController;
    private List<Subscription> subscriptions;

    public SubscriptionRecoverer(IOfflineManager offlineManager, RealtimeController realtimeController) {
      this.realtimeController = realtimeController;
      subscriptions = new List<Subscription>();
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
    /// Remove every subscriptions that satisfy the predicate.
    /// </summary>
    public void Remove(Predicate<Subscription> predicate) {
      lock (subscriptions) {
        subscriptions.RemoveAll(predicate);
      }
    }

    /// <summary>
    /// Clear every subscriptions saved.
    /// </summary>
    public void ClearAllSubscriptions() {
      lock (subscriptions) {
        subscriptions.Clear();
      }
    }

    /// <summary>
    /// Renew one subscription.
    /// </summary>
    private async Task RenewSubscription(Subscription subscription) {

      string roomId = await realtimeController.RecovererSubscribe(
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
    public void RenewSubscriptions() {
      lock (subscriptions) {
        for (int i = 0; i < subscriptions.Count; i++) {
          Subscription subscription = subscriptions[i];
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
          RenewSubscription(subscription);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
      }
    }

  }
}

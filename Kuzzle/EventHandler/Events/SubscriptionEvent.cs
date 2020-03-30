using System;
using KuzzleSdk.Offline.Subscription;

namespace KuzzleSdk.EventHandler.Events {
  /// <summary>
  /// List of possible subscription actions
  /// </summary>
  public enum SubscriptionAction {
    /// <summary>
    /// A subscription was added
    /// </summary>
    Add,
    /// <summary>
    /// A subscription was removed
    /// </summary>
    Remove,
    /// <summary>
    /// Subscriptions were cleared up
    /// </summary>
    Clear,
  }

  /// <summary>
  /// Event triggered on realtime subscriptions
  /// </summary>
  public class SubscriptionEvent : EventArgs {
    /// <summary>
    /// Explains what subscription action triggered the SubscriptionEvent event
    /// </summary>
    public SubscriptionAction Action { get; private set; }

    /// <summary>
    /// Event triggered on realtime subscriptions
    /// </summary>
    public SubscriptionEvent(SubscriptionAction action) {
      Action = action;
    }
  }
}

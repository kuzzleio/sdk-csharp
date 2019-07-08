using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "realtime" Kuzzle API controller
  /// </summary>
  public sealed class RealtimeController : BaseController {
    /// <summary>
    /// Delegate to provide to the SubscribeAsync method
    /// </summary>
    public delegate void NotificationHandler(Response notification);

    // rooms => channels
    private readonly Dictionary<string, HashSet<string>> rooms =
      new Dictionary<string, HashSet<string>>();

    // channels => handlers
    private readonly Dictionary<string, List<Tuple<NotificationHandler, SubscribeOptions>>>
      channels = new Dictionary<string, List<Tuple<NotificationHandler, SubscribeOptions>>>();

    private void NotificationsListener(object sender, Response notification) {
      if (notification.Type == "TokenExpired") {
        api.DispatchTokenExpired();
        return;
      }

      var id = notification.Room;
      string sdkInstanceId = (string)notification.Volatile?["sdkInstanceId"];

      if (channels.ContainsKey(id)) {
        foreach (Tuple<NotificationHandler, SubscribeOptions> n in channels[id]) {
          if (
            n.Item2.SubscribeToSelf ||
            sdkInstanceId == null ||
            sdkInstanceId != api.InstanceId
          ) {
            n.Item1(notification);
          }
        }
      }
    }

    private void ClearAllSubscriptions() {
      api.GetOfflineManager().GetSubscriptionRecoverer().ClearAllSubscriptions();
      rooms.Clear();
      channels.Clear();
    }

    private void TokenExpiredListener() {
      ClearAllSubscriptions();
    }

    private void StateChangeListener(object sender, ProtocolState state) {
      if (state == ProtocolState.Closed) {
        ClearAllSubscriptions();
      }
    }

    private void AddNotificationHandler(
        string room,
        string channel,
        NotificationHandler h,
        SubscribeOptions options) {
      if (!rooms.ContainsKey(room)) {
        rooms[room] = new HashSet<string>();
      }

      rooms[room].Add(channel);

      if (!channels.ContainsKey(channel)) {
        channels[channel] =
          new List<Tuple<NotificationHandler, SubscribeOptions>>();
      }

      channels[channel].Add(new Tuple<NotificationHandler, SubscribeOptions>(
        h, options));
    }

    private void DelNotificationHandlers(string room) {
      api.GetOfflineManager().GetSubscriptionRecoverer().Remove((obj) => obj.RoomId == room);
      foreach (string channel in rooms[room]) {
        channels.Remove(channel);
      }

      rooms.Remove(room);
    }

    internal RealtimeController(IKuzzleApi api) : base(api) {
      api.UnhandledResponse += NotificationsListener;
      api.NetworkProtocol.StateChanged += StateChangeListener;
      api.TokenExpired += TokenExpiredListener;
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations 
    /// before the <see cref="T:KuzzleSdk.API.Controllers.RealtimeController"/>
    /// is reclaimed by garbage collection.
    /// </summary>
    ~RealtimeController() {
      api.UnhandledResponse -= NotificationsListener;
      api.NetworkProtocol.StateChanged -= StateChangeListener;
    }

    /// <summary>
    /// Returns the number of other connections sharing the same subscription.
    /// </summary>
    public async Task<int> CountAsync(string roomId) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "realtime" },
        { "action", "count" },
        { "body", new JObject{ { "roomId", roomId } } }
      });
      return (int)response.Result["count"];
    }

    /// <summary>
    /// Sends a real-time message to Kuzzle. The message will be dispatched to 
    /// all clients with subscriptions matching the index, the collection and 
    /// the message content.
    /// </summary>
    public async Task PublishAsync(
        string index, string collection, JObject message) {
      await api.QueryAsync(new JObject {
        { "controller", "realtime" },
        { "action", "publish" },
        { "index", index },
        { "collection", collection },
        { "body", message }
      });
    }

    /// <summary>
    /// Subscribes by providing a set of filters: messages, document changes 
    /// and, optionally, user events matching the provided filters will 
    /// generate real-time notifications, sent to you in real-time by Kuzzle.
    /// </summary>
    public async Task<string> SubscribeAsync(
        string index, string collection, JObject filters,
        NotificationHandler handler, SubscribeOptions options = null) {
      string roomId = await RecovererSubscribe(index, collection, filters, handler, options);
      return roomId;
    }

    /// <summary>
    /// Removes a subscription.
    /// </summary>
    public async Task UnsubscribeAsync(string roomId) {
      await api.QueryAsync(new JObject {
        { "controller", "realtime" },
        { "action", "unsubscribe" },
        { "body", new JObject{ { "roomId", roomId } } }
      });

      DelNotificationHandlers(roomId);
    }

    /// <summary>
    /// Subscribes by providing a set of filters: messages, document changes 
    /// and, optionally, user events matching the provided filters will 
    /// generate real-time notifications, sent to you in real-time by Kuzzle.
    /// and add the Subscription to the SubscriptionRecoverer for Offline Mode
    /// </summary>
    /// <param name="addToRecoverer">If set to <c>true</c> add to recoverer.</param>
    internal async Task<string> RecovererSubscribe(
        string index, string collection, JObject filters,
        NotificationHandler handler, SubscribeOptions options = null, bool addToRecoverer = true) {
      var request = new JObject {
        { "controller", "realtime" },
        { "action", "subscribe" },
        { "index", index },
        { "collection", collection },
        { "body", filters }
      };

      if (options != null) {
        request.Merge(JObject.FromObject(options));
      }

      Response response = await api.QueryAsync(request);

      AddNotificationHandler(
        (string)response.Result["roomId"],
        (string)response.Result["channel"],
        handler,
        options ?? new SubscribeOptions());

      if (addToRecoverer) {
        api.GetOfflineManager().GetSubscriptionRecoverer().Add(new Subscription(
          index,
          collection,
          filters,
          handler,
          (string)response.Result["roomId"],
          (string)response.Result["channel"],
          options
        ));
      }

      return (string)response.Result["roomId"];
    }

  }
}

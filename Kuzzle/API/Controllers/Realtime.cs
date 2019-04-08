using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kuzzle.API.Controllers {
  public sealed class Realtime : Base {
    public delegate void NotificationHandler(Response notification);

    // rooms => channels
    private readonly Dictionary<string, List<string>> rooms =
      new Dictionary<string, List<string>>();

    // channels => handlers
    private readonly Dictionary<string, List<Tuple<NotificationHandler, SubscribeOptions>>>
      channels = new Dictionary<string, List<Tuple<NotificationHandler, SubscribeOptions>>>();

    private void NotificationsListener(object sender, Response notification) {
      var id = notification.Room;
      string sdkInstanceId = (string)notification.Volatile?["sdkInstanceId"];

      if (channels.ContainsKey(id)) {
        foreach (Tuple<NotificationHandler, SubscribeOptions> n in channels[id]) {
          if (n.Item2.SubscribeToSelf || sdkInstanceId == null
              || sdkInstanceId != kuzzle.InstanceId) {
            n.Item1(notification);
          }
        }
      }
    }

    private void AddNotificationHandler(
        string room,
        string channel,
        NotificationHandler h,
        SubscribeOptions options) {
      if (!rooms.ContainsKey(room)) {
        rooms[room] = new List<string>();
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
      foreach (string channel in rooms[room]) {
        channels.Remove(channel);
      }

      rooms.Remove(room);
    }

    internal Realtime(Kuzzle k) : base(k) {
      kuzzle.UnhandledResponse += NotificationsListener;
    }

    ~Realtime() {
      kuzzle.UnhandledResponse -= NotificationsListener;
    }

    /// <summary>
    /// Returns the number of other connections sharing the same subscription.
    /// </summary>
    public async Task<int> CountAsync(string roomId) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "realtime" },
        { "action", "count" },
        { "body", new JObject{ { "roomId", roomId } } }
      });

      return (int)response.Result;
    }

    /// <summary>
    /// Sends a real-time message to Kuzzle. The message will be dispatched to 
    /// all clients with subscriptions matching the index, the collection and 
    /// the message content.
    /// </summary>
    public async Task PublishAsync(
        string index, string collection, JObject message) {
      await kuzzle.Query(new JObject {
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
      var request = new JObject {
        { "controller", "realtime" },
        { "action", "subscribe" },
        { "index", index },
        { "collection", collection },
        { "body", filters }
      };

      if (options != null) {
        request.Merge(options);
      }

      Response response = await kuzzle.Query(request);

      AddNotificationHandler(
        (string)response.Result["roomId"],
        (string)response.Result["channel"],
        handler,
        options ?? new SubscribeOptions());

      return (string)response.Result["roomId"];
    }

    /// <summary>
    /// Removes a subscription.
    /// </summary>
    public async Task UnsubscribeAsync(string roomId) {
      await kuzzle.Query(new JObject {
        { "controller", "realtime" },
        { "action", "unsubscribe" },
        { "body", new JObject{ { "roomId", roomId } } }
      });

      DelNotificationHandlers(roomId);
    }
  }

  [JsonObject(MemberSerialization.OptIn)]
  public class SubscribeOptions {
    [JsonProperty(PropertyName = "scope")]
    public string Scope = "all";

    [JsonProperty(PropertyName = "users")]
    public string Users = "all";

    [JsonProperty(PropertyName = "volatile")]
    public JObject Volatile;

    // not serialized on purpose: not a Kuzzle API option
    public bool SubscribeToSelf = true;
  }
}

using System;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;
using static KuzzleSdk.API.Controllers.RealtimeController;

namespace KuzzleSdk.Offline.Subscription {

  public class Subscription {

    public string Index { get; private set; }
    public string Collection { get; private set; }
    public JObject Filters { get; private set; }
    public NotificationHandler Handler { get; private set; }
    public string RoomId { get; set; }
    public string Channel { get; private set; }
    public SubscribeOptions Options { get; private set; }

    public Subscription(string index, string collection, JObject filters,
        NotificationHandler handler, string roomId, string channel, SubscribeOptions options = null) {
      Index = index;
      Collection = collection;
      Filters = filters;
      Handler = handler;
      RoomId = roomId;
      Channel = channel;
      Options = options;
    }
  }
}

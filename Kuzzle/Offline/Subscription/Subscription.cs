using System;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;
using static KuzzleSdk.API.Controllers.RealtimeController;

namespace KuzzleSdk.Offline.Subscription {
  /// <summary>
  /// Subscription class
  /// </summary>
  public class Subscription {
    /// <summary>
    /// Real-time index
    /// </summary>
    public string Index { get; private set; }

    /// <summary>
    /// Real-time collection
    /// </summary>
    public string Collection { get; private set; }

    /// <summary>
    /// Subscription filters (Koncorde format)
    /// </summary>
    public JObject Filters { get; private set; }

    /// <summary>
    /// Callback invoked everytime a notification is received from Kuzzle
    /// </summary>
    public NotificationHandler Handler { get; private set; }

    /// <summary>
    /// Subscription identifier
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// Subscription channel identifier
    /// </summary>
    public string Channel { get; private set; }

    /// <summary>
    /// Susbcription options
    /// </summary>
    public SubscribeOptions Options { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Subscription(
      string index,
      string collection,
      JObject filters,
      NotificationHandler handler,
      string roomId,
      string channel,
      SubscribeOptions options = null
    ) {
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

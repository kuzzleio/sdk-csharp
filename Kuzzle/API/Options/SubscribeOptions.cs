using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Options {
  /// <summary>
  /// Options for real-time subscriptions
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class SubscribeOptions {
    /// <summary>
    /// Subscription scope (values: all, out, in)
    /// </summary>
    [JsonProperty(PropertyName = "scope")]
    public string Scope = "all";

    /// <summary>
    /// Filters notifications about users activity (values: all, out, in)
    /// </summary>
    [JsonProperty(PropertyName = "users")]
    public string Users = "all";

    /// <summary>
    /// Pass data to this room's other subscribers, once at the moment of 
    /// subscription, and once when leaving the room (whatever the reason).
    /// </summary>
    [JsonProperty(
      PropertyName = "volatile",
      NullValueHandling = NullValueHandling.Ignore)]
    public JObject Volatile;

    /// <summary>
    /// If true, receive notifications emanating from this SDK instance actions.
    /// </summary>
    // not serialized on purpose: not a Kuzzle API option
    public bool SubscribeToSelf = true;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SubscribeOptions() { }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    public SubscribeOptions(SubscribeOptions src) {
      Scope = string.Copy(src.Scope);
      Users = string.Copy(src.Users);

      if (src.Volatile != null) {
        Volatile = (JObject)src.Volatile.DeepClone();
      }

      SubscribeToSelf = src.SubscribeToSelf;
    }
  }
}

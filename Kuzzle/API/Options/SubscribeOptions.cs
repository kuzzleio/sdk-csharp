using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Options {
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

    public SubscribeOptions() { }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    SubscribeOptions(SubscribeOptions src) {
      Scope = string.Copy(src.Scope);
      Users = string.Copy(src.Users);
      Volatile = (JObject)src.Volatile.DeepClone();
      SubscribeToSelf = src.SubscribeToSelf;
    }
  }
}

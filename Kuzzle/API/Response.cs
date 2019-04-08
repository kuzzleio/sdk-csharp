using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kuzzle.API {
  /// <summary>
  /// Represents a Kuzzle API response
  /// </summary>
  public sealed class Response : EventArgs {
    public struct ErrorResponse {
      public int status;
      public string message;
    }

    /// <summary>
    /// Response factory, creating a Response instance from a 
    /// serialized JSON string
    /// </summary>
    /// <returns>Response class instance</returns>
    /// <param name="serialized">Serialized JSON string</param>
    public static Response FromString(string serialized) {
      return JsonConvert.DeserializeObject<Response>(serialized);
    }

    // The C# compiler complains about this variable not being used and being
    // always null, which is not the case since it's correctly populated by 
    // deserializing a raw JSON object.
#pragma warning disable CS0649
    [JsonProperty(PropertyName = "room")]
    internal readonly string Room;
#pragma warning restore CS0649

    [JsonProperty(PropertyName = "result")]
    public readonly JToken Result;

    [JsonProperty(PropertyName = "error")]
    public readonly ErrorResponse? Error;

    [JsonProperty(PropertyName = "requestId")]
    public readonly string RequestId;

    [JsonProperty(PropertyName = "status")]
    public readonly int Status;

    [JsonProperty(PropertyName = "controller")]
    public readonly string Controller;

    [JsonProperty(PropertyName = "action")]
    public readonly string Action;

    [JsonProperty(PropertyName = "index")]
    public readonly string Index;

    [JsonProperty(PropertyName = "collection")]
    public readonly string Collection;

    [JsonProperty(PropertyName = "volatile")]
    public readonly JObject Volatile;

    // The following properties are specific to real-time notifications

    [JsonProperty(PropertyName = "protocol")]
    public readonly string Protocol;

    [JsonProperty(PropertyName = "scope")]
    public readonly string Scope;

    [JsonProperty(PropertyName = "state")]
    public readonly string State;

    [JsonProperty(PropertyName = "timestamp")]
    public readonly long? Timestamp;

    [JsonProperty(PropertyName = "type")]
    public readonly string Type;
  }
}

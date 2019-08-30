using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API {
  /// <summary>
  /// Represents a Kuzzle API response
  /// </summary>
  public sealed class Response : EventArgs {
    /// <summary>
    /// Represents a Kuzzle API error
    /// </summary>
    public class ErrorResponse {
      /// <summary>
      /// Response status, following HTTP status codes
      /// </summary>
      [JsonProperty(PropertyName = "status")]
      public int Status;

      /// <summary>
      /// Error message
      /// </summary>
      [JsonProperty(PropertyName = "message")]
      public string Message;

      /// <summary>
      /// Error message
      /// </summary>
      [JsonProperty(PropertyName = "stack")]
      public string Stack;
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

    /// <summary>
    /// Response payload (depends on the executed API action)
    /// </summary>
    [JsonProperty(
      PropertyName = "result",
      NullValueHandling = NullValueHandling.Ignore)]
    public readonly JToken Result;

    /// <summary>
    /// Error object (null if the request finished successfully)
    /// </summary>
    [JsonProperty(
      PropertyName = "error",
      NullValueHandling = NullValueHandling.Ignore)]
    public readonly ErrorResponse Error;

    /// <summary>
    /// Request unique identifier.
    /// </summary>
    [JsonProperty(PropertyName = "requestId")]
    public readonly string RequestId;

    /// <summary>
    /// Response status, following HTTP status codes
    /// </summary>
    [JsonProperty(PropertyName = "status")]
    public readonly int Status;

    /// <summary>
    /// Executed Kuzzle API controller.
    /// </summary>
    [JsonProperty(PropertyName = "controller")]
    public readonly string Controller;

    /// <summary>
    /// Executed Kuzzle API controller's action.
    /// </summary>
    [JsonProperty(PropertyName = "action")]
    public readonly string Action;

    /// <summary>
    /// Impacted data index.
    /// </summary>
    [JsonProperty(PropertyName = "index")]
    public readonly string Index;

    /// <summary>
    /// Impacted data collection.
    /// </summary>
    [JsonProperty(PropertyName = "collection")]
    public readonly string Collection;

    /// <summary>
    /// Volatile data.
    /// </summary>
    [JsonProperty(
      PropertyName = "volatile",
      NullValueHandling = NullValueHandling.Ignore)]
    public readonly JObject Volatile;

    // The following properties are specific to real-time notifications

    /// <summary>
    /// Network protocol at the origin of the real-time notification.
    /// </summary>
    [JsonProperty(PropertyName = "protocol")]
    public readonly string Protocol;

    /// <summary>
    /// Document scope ("in" or "out")
    /// </summary>
    [JsonProperty(PropertyName = "scope")]
    public readonly string Scope;

    /// <summary>
    /// Document state
    /// </summary>
    [JsonProperty(PropertyName = "state")]
    public readonly string State;

    /// <summary>
    /// Notification timestamp (UTC)
    /// </summary>
    [JsonProperty(PropertyName = "timestamp")]
    public readonly long? Timestamp;

    /// <summary>
    /// Notification type
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public readonly string Type;
  }
}

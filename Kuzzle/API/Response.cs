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
    /// ApiResponse factory, creating an ApiResponse instance from a 
    /// serialized JSON string
    /// </summary>
    /// <returns>ApiResponse class instance</returns>
    /// <param name="serialized">Serialized JSON string</param>
    public static Response FromString(string serialized) {
      return JsonConvert.DeserializeObject<Response>(serialized);
    }

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
  }
}

using Newtonsoft.Json.Linq;

namespace Kuzzle {
  /// <summary>
  /// Represents a Kuzzle API response
  /// </summary>
  public struct Response {
    public struct ErrorResponse {
      public int status;
      public string message;
    }

    public JObject result;
    public ErrorResponse error;
    public string requestId;
    public int status;
    public string controller;
    public string action;
    public string index;
    public string collection;
  }
}

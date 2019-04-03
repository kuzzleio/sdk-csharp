using System;
using Newtonsoft.Json.Linq;

namespace Kuzzle {
  /// <summary>
  /// Represents a Kuzzle API response
  /// </summary>
  public sealed class ApiResponse : EventArgs {
    public struct ErrorResponse {
      public readonly int status;
      public readonly string message;
    }

    public readonly JObject Result;
    public readonly ErrorResponse Error;
    public readonly string RequestId;
    public readonly int Status;
    public readonly string Controller;
    public readonly string Action;
    public readonly string Index;
    public readonly string Collection;
  }
}

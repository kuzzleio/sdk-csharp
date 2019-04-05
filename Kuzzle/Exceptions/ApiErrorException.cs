using System;
namespace Kuzzle.Exceptions {
  public class ApiErrorException : Exception {
    /// <summary>
    /// Kuzzle API error code
    /// </summary>
    public int Status { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.ApiError"/> class
    /// </summary>
    /// <param name="response">Kuzzle API Response.</param>
    public ApiErrorException(ApiResponse response)
        : base(response.Error?.message) {
      Status = response.Status;
    }
  }
}

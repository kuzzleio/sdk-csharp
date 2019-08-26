namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Passed to async tasks when an API request returns an error.
  /// </summary>
  public class ApiErrorException : KuzzleException {
    /// <summary>
    /// Kuzzle API stack trace
    /// </summary>
    public string Stack { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.ApiError"/> class
    /// </summary>
    /// <param name="response">Kuzzle API Response.</param>
    public ApiErrorException(API.Response response)
        : base(response.Error?.Message, response.Status) {
      Stack = response.Error?.Stack;
    }
  }
}

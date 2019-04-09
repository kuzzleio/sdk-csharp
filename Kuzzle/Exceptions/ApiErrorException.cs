namespace KuzzleSdk.Exceptions {
  public class ApiErrorException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.ApiError"/> class
    /// </summary>
    /// <param name="response">Kuzzle API Response.</param>
    public ApiErrorException(API.Response response)
        : base(response.Error?.Message, response.Status) {
    }
  }
}

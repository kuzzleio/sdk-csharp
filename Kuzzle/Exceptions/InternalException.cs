namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Passed to async tasks when an API request returns an error.
  /// </summary>
  public class InternalException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.Internal"/> class
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="status">Error status (HTTP error code)</param>
    public InternalException(string message, int status)
        : base(message, status)
    {
    }
  }
}

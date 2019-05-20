namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Root of all Kuzzle exceptions
  /// </summary>
  public abstract class KuzzleException : System.Exception {
    /// <summary>
    /// Kuzzle API error code
    /// </summary>
    public int Status { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Exceptions.KuzzleException"/> class.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="status">Status.</param>
    protected KuzzleException(string message, int status) : base(message) {
      Status = status;
    }
  }
}

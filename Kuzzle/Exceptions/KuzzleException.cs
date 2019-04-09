namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Root of all Kuzzle exceptions
  /// </summary>
  public abstract class KuzzleException : System.Exception {
    /// <summary>
    /// Kuzzle API error code
    /// </summary>
    public int Status { get; protected set; }

    protected KuzzleException(string message, int status) : base(message) {
      Status = status;
    }
  }
}

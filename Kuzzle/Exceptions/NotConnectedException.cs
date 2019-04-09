namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Thrown when attempting to interact with the network while not connected.
  /// </summary>
  public class NotConnectedException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Exceptions.NotConnectedException"/> class.
    /// </summary>
    public NotConnectedException() : base("Not connected.", 500) {
    }
  }
}

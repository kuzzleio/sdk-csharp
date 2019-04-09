namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Thrown when attempting to interact with the network while not connected.
  /// </summary>
  public class NotConnectedException : KuzzleException {
    public NotConnectedException() : base("Not connected.", 500) {
    }
  }
}

using System;
namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Thrown to close ongoing API tasks, when the connection has been lost while
  /// waiting for a result.
  /// </summary>
  public class ConnectionLostException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Exceptions.ConnectionLostException"/> class.
    /// </summary>
    public ConnectionLostException() : base("Connection lost", 500) {
    }
  }
}

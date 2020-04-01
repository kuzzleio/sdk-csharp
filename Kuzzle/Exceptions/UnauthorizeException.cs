using System;
namespace KuzzleSdk.Exceptions {
  /// <summary>
  /// Thrown when attempting to perform an unauthorized action.
  /// </summary>
  public class UnauthorizeException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.Internal"/> class
    /// </summary>
    /// <param name="message">Error message.</param>
    public UnauthorizeException(string message)
        : base(message, 401) {
    }
  }
}


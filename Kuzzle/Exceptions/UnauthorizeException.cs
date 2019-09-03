using System;
namespace KuzzleSdk.Exceptions {
  public class UnauthorizeException : KuzzleException {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Kuzzle.Exceptions.Internal"/> class
    /// </summary>
    /// <param name="response">Kuzzle API Response.</param>
    public UnauthorizeException(string message)
        : base(message, 401) {
    }
  }
}


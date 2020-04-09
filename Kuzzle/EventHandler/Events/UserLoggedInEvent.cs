using System;
namespace KuzzleSdk.EventHandler.Events {
  /// <summary>
  /// Event triggered on a successful login
  /// </summary>
  public class UserLoggedInEvent : EventArgs {
    /// <summary>
    /// User kuid
    /// </summary>
    public string Kuid { get; private set; }

    /// <summary>
    /// Event triggered on a successful login
    /// </summary>
    public UserLoggedInEvent(string kuid) {
      Kuid = kuid;
    }
  }
}

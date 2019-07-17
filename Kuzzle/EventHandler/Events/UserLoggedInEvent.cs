using System;
namespace KuzzleSdk.EventHandler.Events {
  public class UserLoggedInEvent : EventArgs {

    public string Username { get; private set; }

    public UserLoggedInEvent(string username) {
      Username = username;
    }
  }
}

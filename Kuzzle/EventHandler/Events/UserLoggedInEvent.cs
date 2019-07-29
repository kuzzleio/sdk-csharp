using System;
namespace KuzzleSdk.EventHandler.Events {
  public class UserLoggedInEvent : EventArgs {

    public string Kuid { get; private set; }

    public UserLoggedInEvent(string kuid) {
      Kuid = kuid;
    }
  }
}

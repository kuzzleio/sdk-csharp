namespace KuzzleSdk.EventHandler.Events {
  public class UserChangedEvent {
    public string Username { get; private set; }
    public UserChangedEvent(string username) {
      Username = username;
    }
  }
}

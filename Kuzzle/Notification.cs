using Newtonsoft.Json.Linq;

namespace Kuzzle {
  /// <summary>
  /// Represents a real-time notification
  /// </summary>
  public struct Notification {
    public struct NotificationResult {
      public string _id;
      public uint? count;
      public JObject _source;
    }

    public NotificationResult result;
    public JObject content;
    public JObject volatile_;
    public string action;
    public string collection;
    public string controller;
    public string index;
    public string protocol;
    public string room;
    public string scope;
    public ulong timestamp;
    public string type;
    public string user;
  }
}

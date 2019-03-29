using System.Net.WebSockets;
using Kuzzle.Protocol;

namespace Kuzzle.Protocol {
  public struct WebSocketOptions {
    public int? Port;
    private bool? ssl;

    public bool Ssl {
      get { return ssl ?? false; }
      set { ssl = value; }
    }
  }

  public class WebSocket : AbstractProtocol {
    private readonly string Hostname;

    public WebSocket(string hostname, WebSocketOptions options) {
      Hostname = hostname;
    }
  }
}

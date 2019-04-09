using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Protocol {
  /// <summary>
  /// WebSocket options.
  /// </summary>
  public struct WebSocketOptions {
    private int? port;
    private bool? ssl;
    private int? connectTimeout;

    /// <summary>
    /// If true, connects using SSL.
    /// </summary>
    public bool Ssl {
      get { return ssl ?? false; }
      set { ssl = value; }
    }

    /// <summary>
    /// Kuzzle server port.
    /// </summary>
    public int Port {
      get { return port ?? 7512; }
      set { port = value; }
    }

    /// <summary>
    /// Timeout (in milliseconds) after which a connection attempt aborts.
    /// </summary>
    public int ConnectTimeout {
      get { return connectTimeout ?? 30000; }
      set { connectTimeout = value; }
    }
  }

  /// <summary>
  /// WebSocket network protocol.
  /// </summary>
  public class WebSocket : AbstractProtocol {
    private readonly string hostname;
    private ClientWebSocket socket;
    private WebSocketOptions options;
    private CancellationTokenSource receiveCancellationToken;

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Protocol.WebSocket"/> class.
    /// </summary>
    /// <param name="hostname">Kuzzle hostname (or IP address).</param>
    public WebSocket(string hostname) : this(hostname, new WebSocketOptions()) {
      State = ProtocolState.Closed;
    }

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Protocol.WebSocket"/> class.
    /// </summary>
    /// <param name="hostname">Kuzzle hostname (or IP address).</param>
    /// <param name="options">Connection options.</param>
    public WebSocket(string hostname, WebSocketOptions options) {
      this.hostname = hostname;
      this.options = options;
      socket = new ClientWebSocket();
    }

    /// <summary>
    /// Connects to a Kuzzle server.
    /// </summary>
    public override async Task ConnectAsync() {
      if (socket?.State == WebSocketState.Connecting
          || socket?.State == WebSocketState.Open) {
        return;
      }

      Uri uri = new Uri("ws" + (options.Ssl ? "s" : "") + "://"
        + hostname + ":" + options.Port);

      CancellationTokenSource source =
          new CancellationTokenSource(options.ConnectTimeout);

      await socket.ConnectAsync(uri, source.Token);

      State = ProtocolState.Open;
      DispatchStateChange(State);

      Listen();
    }

    /// <summary>
    /// Disconnects this instance.
    /// </summary>
    public override void Disconnect() {
      receiveCancellationToken?.Cancel();
      socket?.Abort();
      CloseState();
    }

    /// <summary>
    /// Sends a formatted API request to a Kuzzle server.
    /// </summary>
    public override async Task SendAsync(JObject payload) {
      var buffer = Encoding.UTF8.GetBytes(payload.ToString());

      if (State == ProtocolState.Closed) {
        CloseState();
      } else {
        await socket?.SendAsync(
          new ArraySegment<byte>(buffer),
          WebSocketMessageType.Text,
          true,
          CancellationToken.None);
      }
    }

    private void Listen() {
      receiveCancellationToken = new CancellationTokenSource();

      Task.Run(async () => {
        byte[] buffer = new byte[255];
        var segment = new ArraySegment<byte>(buffer);
        WebSocketReceiveResult data;

        while (socket.State == WebSocketState.Open) {
          string message = "";

          do {
            data = await socket.ReceiveAsync(
              segment,
              receiveCancellationToken.Token);
            message += Encoding.UTF8.GetString(buffer, 0, data.Count);
          } while (!data.EndOfMessage);

          if (message.Length > 0) {
            DispatchResponse(message);
          }
        }
        CloseState();
      }, receiveCancellationToken.Token);
    }

    private void CloseState() {
      if (State != ProtocolState.Closed) {
        State = ProtocolState.Closed;
        DispatchStateChange(State);
      }
    }
  }
}

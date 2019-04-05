using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Protocol {
  public struct WebSocketOptions {
    private int? port;
    private bool? ssl;
    private Int32? connectTimeout;

    public bool Ssl {
      get { return ssl ?? false; }
      set { ssl = value; }
    }

    public int Port {
      get { return port ?? 7512; }
      set { port = value; }
    }

    public Int32 ConnectTimeout {
      get { return connectTimeout ?? 30000; }
      set { connectTimeout = value; }
    }
  }

  public class WebSocket : AbstractProtocol {
    private readonly string hostname;
    private ClientWebSocket socket;
    private WebSocketOptions options;
    private CancellationTokenSource receiveCancellationToken;

    public WebSocket(string hostname) : this(hostname, new WebSocketOptions()) {
    }

    public WebSocket(string hostname, WebSocketOptions options) {
      this.hostname = hostname;
      this.options = options;
      socket = new ClientWebSocket();
    }

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
      Listen();
    }

    public override void Disconnect() {
      receiveCancellationToken?.Cancel();
      socket?.Abort();
    }

    public override async Task SendAsync(JObject payload) {
      var buffer = Encoding.UTF8.GetBytes(payload.ToString());

      await socket?.SendAsync(
        new ArraySegment<byte>(buffer),
        WebSocketMessageType.Text,
        true,
        CancellationToken.None);
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
            data = await socket.ReceiveAsync(segment, CancellationToken.None);
            message += Encoding.UTF8.GetString(buffer, 0, data.Count);
          } while (!data.EndOfMessage);

          if (message.Length > 0) {
            DispatchResponse(message);
          }

          await Task.Delay(200);
        }
      }, receiveCancellationToken.Token);
    }
  }
}

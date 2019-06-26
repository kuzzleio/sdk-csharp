using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Protocol {
  /// <summary>
  /// WebSocket network protocol.
  /// </summary>
  public class WebSocket : AbstractProtocol {
    private const int receiveBufferSize = 64 * 1024;
    private const int sendBufferSize = 8 * 1024;
    private readonly Uri uri;
    private readonly ClientWebSocket socket;
    private readonly CancellationToken connectTimeout;
    private CancellationTokenSource receiveCancellationToken;
    private CancellationTokenSource sendCancellationToken;
    private ArraySegment<byte> incomingBuffer =
      System.Net.WebSockets.WebSocket.CreateClientBuffer(
        receiveBufferSize, sendBufferSize);
    private readonly BlockingCollection<JObject> sendQueue =
      new BlockingCollection<JObject>();

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Protocol.WebSocket"/> class.
    /// </summary>
    /// <param name="uri">URI pointing to a Kuzzle endpoint.</param>
    /// <param name="cancellationToken">Connection cancellation token</param>
    public WebSocket(Uri uri, CancellationToken cancellationToken) {
      this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
      connectTimeout = cancellationToken;
      State = ProtocolState.Closed;
      socket = new ClientWebSocket();
      socket.Options.SetBuffer(receiveBufferSize, sendBufferSize);
    }

    /// <summary>
    /// Connects to a Kuzzle server.
    /// </summary>
    public override async Task ConnectAsync() {
      if (
        socket.State == WebSocketState.Connecting ||
        socket.State == WebSocketState.Open
      ) {
        return;
      }

      await socket.ConnectAsync(uri, connectTimeout);

      State = ProtocolState.Open;
      DispatchStateChange(State);

      Dequeue();
      Listen();
    }

    /// <summary>
    /// Disconnects this instance.
    /// </summary>
    public override void Disconnect() {
      socket.Abort();
      CloseState();
    }

    /// <summary>
    /// Sends a formatted API request to a Kuzzle server.
    /// </summary>
    public override void Send(JObject payload) {
      sendQueue.Add(payload);
    }

    private void Dequeue() {
      sendCancellationToken = new CancellationTokenSource();

      Task.Run(async () => {
        while (socket.State == WebSocketState.Open) {
          var payload = sendQueue.Take();
          var buffer = Encoding.UTF8.GetBytes(payload.ToString());

          await socket.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            sendCancellationToken.Token);
        }
      }, sendCancellationToken.Token);
    }

    private void Listen() {
      receiveCancellationToken = new CancellationTokenSource();

      Task.Run(async () => {
        WebSocketReceiveResult wsResult;
        StringBuilder messageBuilder = new StringBuilder(receiveBufferSize * 2);

        while (socket.State == WebSocketState.Open) {
          string message;

          do {
            wsResult = await socket.ReceiveAsync(
              incomingBuffer,
              receiveCancellationToken.Token);

            message = Encoding.UTF8.GetString(
              incomingBuffer.Array, 0, wsResult.Count);

            if (!wsResult.EndOfMessage || messageBuilder.Length > 0) {
              messageBuilder.Append(message);
            }
          } while (!wsResult.EndOfMessage);

          if (messageBuilder.Length > 0) {
            message = messageBuilder.ToString();
            messageBuilder.Clear();
          }

          if (!string.IsNullOrEmpty(message)) {
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
        receiveCancellationToken?.Cancel();
        receiveCancellationToken = null;
        sendCancellationToken?.Cancel();
        sendCancellationToken = null;
      }
    }
  }
}

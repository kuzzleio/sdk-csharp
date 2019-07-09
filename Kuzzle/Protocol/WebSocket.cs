using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Protocol {
  /// <summary>
  /// Interface exposing the same properties as ClientWebSocket, to make
  /// our WebSocket class testable via duck typing.
  /// </summary>
  internal interface IClientWebSocket {
    WebSocketState State { get; }

    Task ConnectAsync(Uri uri, CancellationToken cancellationToken);
    Task SendAsync(
      ArraySegment<byte> buffer,
      WebSocketMessageType messageType,
      bool endOfMessage,
      CancellationToken cancellationToken);
    Task<WebSocketReceiveResult> ReceiveAsync(
      ArraySegment<byte> buffer,
      CancellationToken cancellationToken);
    void Abort();
  }

  /// <summary>
  /// WebSocket network protocol.
  /// </summary>
  public class WebSocket : AbstractProtocol {
    private const int receiveBufferSize = 64 * 1024;
    private const int sendBufferSize = 8 * 1024;
    internal dynamic socket;
    private readonly Uri uri;
    private CancellationTokenSource receiveCancellationToken;
    private CancellationTokenSource sendCancellationToken;
    private CancellationTokenSource reconnectCancellationToken;
    private ArraySegment<byte> incomingBuffer =
      System.Net.WebSockets.WebSocket.CreateClientBuffer(
        receiveBufferSize, sendBufferSize);
    private readonly BlockingCollection<JObject> sendQueue =
      new BlockingCollection<JObject>();

    internal virtual dynamic CreateClientSocket() {
      var s = new ClientWebSocket();

      s.Options.SetBuffer(receiveBufferSize, sendBufferSize);
      return s;
    }

    private bool keepAlive = false;
    private bool autoReconnect = false;
    private int reconnectionDelay = 1000;
    private int reconnectionRetries = 20;

    /// <summary>
    /// Actively keep the connection alive
    /// </summary>
    public bool KeepAlive {
      get { return keepAlive; }
      set { keepAlive = value; }
    }

    /// <summary>
    /// Try to reestablish connection on an unexpected network loss
    /// </summary>
    public bool AutoReconnect {
      get { return autoReconnect; }
      set { autoReconnect = value; }
    }

    /// <summary>
    /// The number of milliseconds between 2 automatic reconnections attempts
    /// </summary>
    public int ReconnectionDelay {
      get { return reconnectionDelay; }
      set { reconnectionDelay = value; }
    }

    /// <summary>
    /// The maximum number of automatic reconnections attempts
    /// </summary>
    public int ReconnectionRetries {
      get { return reconnectionRetries; }
      set { reconnectionRetries = value; }
    }

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.Protocol.WebSocket"/> class.
    /// </summary>
    /// <param name="uri">URI pointing to a Kuzzle endpoint.</param>
    public WebSocket(Uri uri) {
      this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
      State = ProtocolState.Closed;
    }

    /// <summary>
    /// Connects to a Kuzzle server.
    /// </summary>
    /// <param name="cancellationToken">Connection cancellation token</param>
    public override async Task ConnectAsync(
      CancellationToken cancellationToken
    ) {
      if (socket != null) {
        return;
      }
        
      socket = CreateClientSocket();

      await socket.ConnectAsync(uri, cancellationToken);

      State = ProtocolState.Open;
      DispatchStateChange(State);

      Dequeue();
      Listen();
    }

    /// <summary>
    /// Disconnects this instance.
    /// </summary>
    public override void Disconnect() {
      CloseState(false);
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
          var payload = sendQueue.Take(sendCancellationToken.Token);
          var buffer = Encoding.UTF8.GetBytes(payload.ToString());
           try {
            await socket.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            sendCancellationToken.Token);
          } catch (Exception e) {
            CloseState(autoReconnect);
            throw e;
          }
        }

      }, CancellationToken.None);
    }

    private void Listen() {
      receiveCancellationToken = new CancellationTokenSource();

      Task.Run(async () => {
        WebSocketReceiveResult wsResult;
        StringBuilder messageBuilder = new StringBuilder(receiveBufferSize * 2);
          while (socket.State == WebSocketState.Open) {
            string message;

          do {
            try {

              wsResult = await socket.ReceiveAsync(
                incomingBuffer,
                receiveCancellationToken.Token);

              message = Encoding.UTF8.GetString(
                incomingBuffer.Array, 0, wsResult.Count);

              if (!wsResult.EndOfMessage || messageBuilder.Length > 0) {
                messageBuilder.Append(message);
              }

            } catch (Exception e) {
              CloseState(autoReconnect);
              throw e;
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
      }, CancellationToken.None);
    }

    public void CancelReconnection() {
      if (reconnectCancellationToken != null) {
        reconnectCancellationToken.Cancel();
      }
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async Task Reconnect() {
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
      if (State == ProtocolState.Reconnecting) {
        return;
      }

      socket.Abort();
      socket = null;
      receiveCancellationToken?.Cancel();
      receiveCancellationToken = null;
      sendCancellationToken?.Cancel();
      sendCancellationToken = null;
      reconnectCancellationToken = new CancellationTokenSource();

      State = ProtocolState.Reconnecting;
      DispatchStateChange(State);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
      Task.Run(async () => {
        for (int i = 0; i < reconnectionRetries; i++) {
          try {
            await ConnectAsync(reconnectCancellationToken.Token);
            return;
          } catch (Exception) {
            socket.Abort();
            socket = null;
            receiveCancellationToken?.Cancel();
            receiveCancellationToken = null;
            sendCancellationToken?.Cancel();
            sendCancellationToken = null;
            await Task.Delay(reconnectionDelay, reconnectCancellationToken.Token);
          }
        }
        CloseState(false);
      }, reconnectCancellationToken.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private void CloseState(bool tryReconnect) {
      if (State != ProtocolState.Closed) {
        if (tryReconnect) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
          Reconnect();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        } else {
          socket.Abort();
          socket = null;
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
}

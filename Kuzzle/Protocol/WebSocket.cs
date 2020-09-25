using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Protocol {
  /// <summary>
  ///Buffer sizes constants
  /// </summary>
  public static class BufferSize {
    /// <summary>
    /// Receive buffer size constant (in bytes)
    /// </summary>
    public const int RECEIVE = 64 * 1024;
    /// <summary>
    /// Send buffer size constant (in bytes)
    /// </summary>
    public const int SEND = 8 * 1024;
  }

  /// <summary>
  /// Interface exposing the same properties as ClientWebSocket, to make
  /// our WebSocket class testable.
  /// </summary>
  public interface IClientWebSocket {
    /// <summary>
    /// Socket state
    /// </summary>
    WebSocketState State { get; }

    /// <summary>
    /// Socket connection method
    /// </summary>
    Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

    /// <summary>
    /// Send a websocket message
    /// </summary>
    Task SendAsync(
      ArraySegment<byte> buffer,
      WebSocketMessageType messageType,
      bool endOfMessage,
      CancellationToken cancellationToken);

    /// <summary>
    /// Receive a websocket message
    /// </summary>
    Task<WebSocketReceiveResult> ReceiveAsync(
      ArraySegment<byte> buffer,
      CancellationToken cancellationToken);

    /// <summary>
    /// Abort the connection and cancel pending operations
    /// </summary>
    void Abort();
  }

  /// <summary>
  /// This is a humble object forcing linkers to keep ClientWebSocket methods
  /// while making the protocal testable with unit tests.
  /// (duck typing with dynamic objets makes linkers think that these methods
  /// are unused and they strip them from the resulting DLL)
  /// </summary>
  public class ClientWebSocketAdapter : IClientWebSocket {
    private ClientWebSocket _ws;

    /// <inheritdoc/>
    public WebSocketState State {
      get { return _ws.State; }
    }

    /// <summary>
    /// Creates a new adapter from a ClientWebSocket class
    /// </summary>
    public ClientWebSocketAdapter () {
      _ws = new ClientWebSocket();
      _ws.Options.SetBuffer(BufferSize.RECEIVE, BufferSize.SEND);
    }

    /// <inheritdoc/>
    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken) {
      return _ws.ConnectAsync(uri, cancellationToken);
    }

    /// <inheritdoc/>
    public Task SendAsync(
      ArraySegment<byte> buffer,
      WebSocketMessageType msgType,
      bool endOfMessage,
      CancellationToken cancellationToken
    ) {
      return _ws.SendAsync(buffer, msgType, endOfMessage, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<WebSocketReceiveResult> ReceiveAsync(
      ArraySegment<byte> buffer,
      CancellationToken cancellationToken
    ) {
      return _ws.ReceiveAsync(buffer, cancellationToken);
    }

    /// <inheritdoc/>
    public void Abort() {
      _ws.Abort();
    }
  }

  /// <summary>
  /// WebSocket network protocol implementation
  /// </summary>
  public class AbstractWebSocket : AbstractProtocol {
    internal IClientWebSocket socket;
    internal Type SocketAdapter;
    private readonly Uri uri;
    private CancellationTokenSource receiveCancellationToken;
    private CancellationTokenSource sendCancellationToken;
    private CancellationTokenSource reconnectCancellationToken;
    private ArraySegment<byte> incomingBuffer =
      System.Net.WebSockets.WebSocket.CreateClientBuffer(
        BufferSize.RECEIVE, BufferSize.SEND);
    private readonly BlockingCollection<JObject> sendQueue =
      new BlockingCollection<JObject>();

    /// <summary>
    /// Actively keep the connection alive
    /// </summary>
    public bool KeepAlive { get; set; } = false;

    /// <summary>
    /// Try to reestablish connection on an unexpected network loss
    /// </summary>
    public bool AutoReconnect { get; set; } = false;

    /// <summary>
    /// The number of milliseconds between 2 automatic reconnections attempts
    /// </summary>
    public int ReconnectionDelay { get; set; } = 1000;

    /// <summary>
    /// The maximum number of automatic reconnections attempts
    /// </summary>
    public int ReconnectionRetries { get; set; } = 20;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:KuzzleSdk.Protocol.AbstractWebSocket"/> class.
    /// </summary>
    /// <param name="SocketAdapter">WebSocket client class to use to instantiate a new socket (must implement IClientWebSocket)</param>
    /// <param name="uri">URI pointing to a Kuzzle endpoint.</param>
    public AbstractWebSocket(Type SocketAdapter, Uri uri) {
      this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
      State = ProtocolState.Closed;
      this.SocketAdapter = SocketAdapter;
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

      socket = (IClientWebSocket)Activator.CreateInstance(SocketAdapter);

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
            CloseState(AutoReconnect);
            throw e;
          }
        }
      }, CancellationToken.None);
    }

    private void Listen() {
      receiveCancellationToken = new CancellationTokenSource();

      Task.Run(async () => {
        WebSocketReceiveResult wsResult;
        StringBuilder messageBuilder = new StringBuilder(BufferSize.RECEIVE * 2);
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
              CloseState(AutoReconnect);
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

    /// <summary>
    /// Cancel an ongoing reconnection
    /// </summary>
    public void CancelReconnection() {
      if (reconnectCancellationToken != null) {
        reconnectCancellationToken.Cancel();
      }
    }

    private void Reconnect() {
      ResetState();
      reconnectCancellationToken = new CancellationTokenSource();

      for (int i = 0; i < ReconnectionRetries; i++) {
        try {
          ConnectAsync(reconnectCancellationToken.Token).Wait();
          return;
        } catch (Exception) {
          ResetState();
          Thread.Sleep(ReconnectionDelay);
        }
      }
      CloseState(false);
    }

    private void ResetState() {
      socket?.Abort();
      socket = null;
      receiveCancellationToken?.Cancel();
      receiveCancellationToken = null;
      sendCancellationToken?.Cancel();
      sendCancellationToken = null;
    }

    private void CloseState(bool tryReconnect) {
      if (State != ProtocolState.Closed) {
        if (tryReconnect) {
          // Handles the protocol state in the main thread to avoid race
          // conditions
          if (State == ProtocolState.Reconnecting) {
            return;
          }

          State = ProtocolState.Reconnecting;

          Thread thread = new Thread(Reconnect);
          thread.Start();

          DispatchStateChange(State);
        } else {
          ResetState();
          State = ProtocolState.Closed;
          DispatchStateChange(State);
        }
      }
    }
  }

  /// <summary>
  /// WebSocket network protocol using System.Net.WebSockets
  /// </summary>
  public class WebSocket : AbstractWebSocket {

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:KuzzleSdk.Protocol.WebSocket"/> class.
    /// </summary>
    /// <param name="uri">URI pointing to a Kuzzle endpoint.</param>
    public WebSocket(Uri uri) : base(typeof(ClientWebSocketAdapter), uri) {}
  }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using KuzzleSdk.Protocol;
using Moq;
using Xunit;

namespace Kuzzle.Tests.Protocol {
  public class MockClientWebSocketAdapter : IClientWebSocket {
    public Mock<IClientWebSocket> mockedSocket;
    public WebSocketState _state = WebSocketState.Open;

    public WebSocketState State {
      get { return _state; }
    }

    public MockClientWebSocketAdapter () {
      mockedSocket = new Mock<IClientWebSocket>();

      mockedSocket
        .Setup(s => s.ConnectAsync(
          It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);
    }

    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken) {
      return mockedSocket.Object.ConnectAsync(uri, cancellationToken);
    }

    public Task SendAsync(
      ArraySegment<byte> buffer,
      WebSocketMessageType msgType,
      bool endOfMessage,
      CancellationToken cancellationToken
    ) {
      return mockedSocket.Object.SendAsync(
        buffer,
        msgType,
        endOfMessage,
        cancellationToken);
    }

    public Task<WebSocketReceiveResult> ReceiveAsync(
      ArraySegment<byte> buffer,
      CancellationToken cancellationToken
    ) {
      Task.Delay(10000).Wait();
      return mockedSocket.Object.ReceiveAsync(buffer, cancellationToken);
    }

    public void Abort() {
      mockedSocket.Object.Abort();
    }
  }

  public class TestableWebSocket : AbstractWebSocket {
    public Mock<IClientWebSocket> mockedSocket;
    public int StateChangesCount = 0;
    public ProtocolState LastStateDispatched = ProtocolState.Closed;

    public TestableWebSocket(Uri uri)
      : base(typeof(MockClientWebSocketAdapter), uri)
    {
      StateChanged += (sender, e) => {
        StateChangesCount++;
        LastStateDispatched = e;
      };
    }

    public override async Task ConnectAsync(
      CancellationToken cancellationToken
    ) {
      await base.ConnectAsync(cancellationToken);
      mockedSocket = ((MockClientWebSocketAdapter)socket).mockedSocket;
    }
  }

  public class WebSocketTest {
    private readonly TestableWebSocket _ws;
    private readonly Uri uri;

    public WebSocketTest() {
      uri = new Uri(@"ws://foo:1234");
      _ws = new TestableWebSocket(uri);
    }

    [Fact]
    public void ConstructorNotConnected() {
      var ws = new TestableWebSocket(uri);

      Assert.Equal(ProtocolState.Closed, ws.State);
      Assert.Null(ws.socket);
    }

    [Fact]
    public void ConstructorRejectsNullUri() {
      Assert.Throws<ArgumentNullException>(() => new TestableWebSocket(null));
    }

    [Fact]
    public async Task ConnectAsyncTest() {
      await _ws.ConnectAsync(CancellationToken.None);

      Assert.NotNull(_ws.socket);

      await _ws.ConnectAsync(CancellationToken.None);
      await _ws.ConnectAsync(CancellationToken.None);
      await _ws.ConnectAsync(CancellationToken.None);

      _ws.mockedSocket.Verify(
        s => s.ConnectAsync(uri, CancellationToken.None),
        Times.Once);

      Assert.Equal(ProtocolState.Open, _ws.State);
      Assert.Equal(1, _ws.StateChangesCount);
      Assert.Equal(ProtocolState.Open, _ws.LastStateDispatched);
    }

    [Fact]
    public async Task DisconnectTest() {
      await _ws.ConnectAsync(CancellationToken.None);

      Assert.Equal(ProtocolState.Open, _ws.State);
      Assert.Equal(1, _ws.StateChangesCount);
      Assert.Equal(ProtocolState.Open, _ws.LastStateDispatched);

      _ws.Disconnect();
      _ws.Disconnect();
      _ws.Disconnect();

      Assert.Equal(ProtocolState.Closed, _ws.State);

      // we should only get 1 state change
      Assert.Equal(2, _ws.StateChangesCount);
      Assert.Equal(ProtocolState.Closed, _ws.LastStateDispatched);

      _ws.mockedSocket.Verify(s => s.Abort(), Times.Once);
    }

    [Fact]
    public void DisconnectWithoutEverConnecting() {
      Assert.Equal(ProtocolState.Closed, _ws.State);
      Assert.Equal(0, _ws.StateChangesCount);

      _ws.Disconnect();

      Assert.Equal(ProtocolState.Closed, _ws.State);
      Assert.Equal(0, _ws.StateChangesCount);
    }
  }
}

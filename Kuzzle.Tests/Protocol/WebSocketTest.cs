using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.Protocol {
  public class TestableWebSocket : WebSocket {
    internal Mock<IClientWebSocket> MockSocket;
    public int StateChangesCount = 0;
    public ProtocolState LastStateDispatched = ProtocolState.Closed;
    public TestableWebSocket(Uri uri) : base(uri) {
      StateChanged += (sender, e) => {
        StateChangesCount++;
        LastStateDispatched = e;
      };
    }

    internal override IClientWebSocket CreateClientSocket() {
      MockSocket = new Mock<IClientWebSocket>();

      MockSocket
        .Setup(s => s.ConnectAsync(
          It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

      MockSocket
        .SetupGet(s => s.State)
        .Returns(System.Net.WebSockets.WebSocketState.Open);

      return MockSocket.Object;
    }
  }

  public class WebSocketTest {
    private readonly TestableWebSocket _ws;
    private readonly Uri uri;

    public WebSocketTest() {
      uri = new Uri(@"ws://foo:1234");
      _ws = new TestableWebSocket(uri);
    }

    /// <summary>
    /// Tries to verify a mock within a timeut
    /// </summary>
    /// <param name="mock">Mock</param>
    /// <param name="timeout">TimeSpan</param>
    private void TryVerify(Mock mock, TimeSpan? timeout = null) {
      TimeSpan _timeout = timeout ?? TimeSpan.FromMilliseconds(1000);
      Exception error = null;

      var task = Task.Run(() => {
        while (true) {
          Thread.Sleep(50);

          try {
            mock.Verify();
            break;
          } catch (Exception e) {
            error = e;
            continue;
          }
        }
      });

      if (!task.Wait(_timeout)) {
        throw new Exception($"Test timed out. Last error: {error.Message}");
      }
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
    public async void ConnectAsyncTest() {
      await _ws.ConnectAsync(CancellationToken.None);

      Assert.NotNull(_ws.socket);

      await _ws.ConnectAsync(CancellationToken.None);
      await _ws.ConnectAsync(CancellationToken.None);
      await _ws.ConnectAsync(CancellationToken.None);

      _ws.MockSocket.Verify(
        s => s.ConnectAsync(uri, CancellationToken.None),
        Times.Once);

      Assert.Equal(ProtocolState.Open, _ws.State);
      Assert.Equal(1, _ws.StateChangesCount);
      Assert.Equal(ProtocolState.Open, _ws.LastStateDispatched);
    }

    [Fact]
    public async void DisconnectTest() {
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

      _ws.MockSocket.Verify(s => s.Abort(), Times.Once);
    }

    [Fact]
    public void DisconnectWithoutEverConnecting() {
      Assert.Equal(ProtocolState.Closed, _ws.State);
      Assert.Equal(0, _ws.StateChangesCount);

      _ws.Disconnect();

      Assert.Equal(ProtocolState.Closed, _ws.State);
      Assert.Equal(0, _ws.StateChangesCount);
    }

    [Fact]
    public async void SendTest() {
      var payload = new JObject { { "foo", "bar" } };

      await _ws.ConnectAsync(CancellationToken.None);
      _ws.MockSocket
        .Setup(s => s.SendAsync(
          It.IsAny<ArraySegment<byte>>(),
          System.Net.WebSockets.WebSocketMessageType.Text,
          true,
          It.IsNotIn<CancellationToken>(new List<CancellationToken> {
            CancellationToken.None
          })))
        .Returns(Task.CompletedTask)
        .Verifiable();

      _ws.Send(payload);

      TryVerify(_ws.MockSocket);
    }
  }
}

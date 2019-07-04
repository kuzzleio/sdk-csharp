using System;
using System.Threading;
using KuzzleSdk.Protocol;
using Moq;
using Xunit;

namespace Kuzzle.Tests.Protocol {
  public class TestableWebSocket : WebSocket {
    public bool SocketCreated { get; private set; } = false;

    public TestableWebSocket(Uri uri) : base(uri) { }

    internal override dynamic CreateClientSocket() {
      SocketCreated = true;
      return new Mock<IClientWebSocket>();
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
      Assert.False(ws.SocketCreated);
    }

    [Fact]
    public void ConstructorRejectsNullUri() {
      Assert.Throws<ArgumentNullException>(() => new TestableWebSocket(null));
    }

    [Fact]
    public async void ConnectAsyncTest() {
      await _ws.ConnectAsync(CancellationToken.None);

      Assert.True(_ws.SocketCreated);
      Mock.Verify(_ws => _ws.ConnectAsync(uri, CancellationToken.None);
    }
  }
}

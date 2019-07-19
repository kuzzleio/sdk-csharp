using Xunit;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using System;
using System.Threading.Tasks;
using Moq;
using System.Threading;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler;

namespace Kuzzle.Tests {
  public class KuzzleTest {
    private readonly KuzzleSdk.Kuzzle _kuzzle;
    private readonly Mock<AbstractProtocol> _protocol;

    public KuzzleTest() {
      _protocol = new Mock<AbstractProtocol>();
      _kuzzle = new KuzzleSdk.Kuzzle(_protocol.Object);
    }

    [Fact]
    public void KuzzleConstructorTest() {
      KuzzleSdk.Kuzzle kuzzle2 = new KuzzleSdk.Kuzzle(_protocol.Object);

      Assert.IsType<AuthController>(_kuzzle.Auth);
      Assert.IsType<CollectionController>(_kuzzle.Collection);
      Assert.IsType<DocumentController>(_kuzzle.Document);
      Assert.IsType<IndexController>(_kuzzle.Index);
      Assert.IsType<RealtimeController>(_kuzzle.Realtime);
      Assert.IsType<ServerController>(_kuzzle.Server);
      Assert.IsType<OfflineManager>(_kuzzle.Offline);
      Assert.IsType<KuzzleEventHandler>(_kuzzle.EventHandler);
      Assert.NotEqual(_kuzzle.InstanceId, kuzzle2.InstanceId);
    }

    [Fact]
    public async void ConnectAsyncTest() {
      await _kuzzle.ConnectAsync(CancellationToken.None);

      _protocol.Verify(m => m.ConnectAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public void DisconnectTest() {
      _kuzzle.Disconnect();

      _protocol.Verify(m => m.Disconnect(), Times.Once);
    }


    [Fact]
    public async void QueryAsyncNullTest() {
      JObject request = null;
      _protocol.Setup(protocol => protocol.Send(It.IsAny<JObject>()));

      await Assert.ThrowsAsync<InternalException>(async () => {
        await _kuzzle.QueryAsync(request);
      });
    }

    [Fact]
    public async void QueryAsyncWrongVolatileTest() {
      JObject request = new JObject {
        { "volatile", "not a JObject" }
      };

      _protocol.Setup(protocol => protocol.Send(It.IsAny<JObject>()));

      await Assert.ThrowsAsync<InternalException>(async () => {
        await _kuzzle.QueryAsync(request);
      });
    }

    [Fact]
    public void QueryAsyncTest() {
      JObject request = new JObject {
        { "controller", "test" },
        { "action", "testAction" }
      };
      _kuzzle.AuthenticationToken = "jwt auth token";
      _protocol.Setup(protocol => protocol.Send(It.IsAny<JObject>()));

      _kuzzle.QueryAsync(request);

      _protocol.Verify(
        protocol => protocol.Send(It.Is<JObject>(o => testSendArg(o))));

      Assert.Single(_kuzzle.requests);
    }
    private bool testSendArg(JObject request) {
      Assert.Equal("test", request["controller"]);
      Assert.Equal("testAction", request["action"]);
      Assert.Equal("jwt auth token", request["jwt"]);
      Assert.Equal(request["volatile"], new JObject {
        { "sdkVersion", _kuzzle.Version },
        { "sdkInstanceId", _kuzzle.InstanceId }
      }, new JTokenEqualityComparer());

      return true;
    }

    [Fact]
    public async void ResponseListenerTest() {
      TaskCompletionSource<Response> responseTask =
        new TaskCompletionSource<Response>();
      string requestId = "uniq-id";
      JObject apiResponse = new JObject {
        { "requestId", requestId },
        { "room", requestId },
        { "status", 200 },
        { "result", "i am the result" }
      };
      _kuzzle.requests[requestId] = responseTask;

      _kuzzle.ResponsesListener(_kuzzle, apiResponse.ToString());
      Response response = await responseTask.Task;

      Assert.False(_kuzzle.requests.ContainsKey(requestId));
      Assert.Equal("i am the result", response.Result);
      Assert.Equal(200, response.Status);
    }

    [Fact]
    public async void ResponseListenerTokenExpiredTest() {
      bool eventDispatched = false;
      _kuzzle.EventHandler.TokenExpired += delegate() {
        eventDispatched = true;
      };
      TaskCompletionSource<Response> responseTask =
        new TaskCompletionSource<Response>();
      string requestId = "uniq-id";
      JObject apiResponse = new JObject {
        { "requestId", requestId },
        { "room", requestId },
        { "status", 401 },
        { "error", new JObject {
          { "message", "Token expired"}
        }}
      };
      _kuzzle.requests[requestId] = responseTask;


      _kuzzle.ResponsesListener(_kuzzle, apiResponse.ToString());
      ApiErrorException ex =
        await Assert.ThrowsAsync<ApiErrorException>(async () => {
          await responseTask.Task;
        });

      Assert.True(eventDispatched);
      Assert.Equal(401, ex.Status);
      Assert.Equal("Token expired", ex.Message);
    }

    [Fact]
    public void ResponseListenerUnhandledTest() {
      bool eventDispatched = false;
      _kuzzle.EventHandler.UnhandledResponse += delegate(object sender, Response response) {
        eventDispatched = true;

        Assert.Equal("i am the result", response.Result);
      };
      string requestId = "uniq-id";
      JObject apiResponse = new JObject {
        { "requestId", requestId },
        { "room", requestId },
        { "status", 200 },
        { "result", "i am the result" }
      };

      _kuzzle.ResponsesListener(_kuzzle, apiResponse.ToString());

      Assert.True(eventDispatched);
    }

    [Fact]
    public async void StateChangeListenerTest() {
      TaskCompletionSource<Response> responseTask1 =
        new TaskCompletionSource<Response>();
      TaskCompletionSource<Response> responseTask2 =
        new TaskCompletionSource<Response>();
      _kuzzle.requests["request-id-1"] = responseTask1;
      _kuzzle.requests["request-id-2"] = responseTask2;

      _kuzzle.StateChangeListener(_kuzzle, ProtocolState.Closed);

      await Assert.ThrowsAsync<ConnectionLostException>(async () => {
        await responseTask1.Task;
      });
      await Assert.ThrowsAsync<ConnectionLostException>(async () => {
        await responseTask2.Task;
      });
      Assert.Empty(_kuzzle.requests.Keys);
    }
  }
}

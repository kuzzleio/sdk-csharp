using Xunit;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;
using Kuzzle.Tests.Protocol;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using System;
using System.Threading.Tasks;
using Moq;

namespace Kuzzle.Tests {
  public class KuzzleTest {
    private readonly KuzzleSdk.Kuzzle _kuzzle;
    private readonly ProtocolMock _protocol;

    public KuzzleTest() {
      _protocol = new ProtocolMock();
      _kuzzle = new KuzzleSdk.Kuzzle(_protocol.MockedObject);
    }

    [Fact]
    public void DispatchTokenExpiredTest() {
      bool eventDispatched = false;

      _kuzzle.TokenExpired += delegate() {
        eventDispatched = true;
      };

      _kuzzle.DispatchTokenExpired();

      Assert.Null(_kuzzle.AuthenticationToken);
      Assert.True(eventDispatched);
    }

    [Fact]
    public void KuzzleConstructorTest() {
      Assert.NotNull(_kuzzle.Auth);
      Assert.NotNull(_kuzzle.Collection);
      Assert.NotNull(_kuzzle.Document);
      Assert.NotNull(_kuzzle.Realtime);
      Assert.NotNull(_kuzzle.Server);

      Assert.NotNull(_kuzzle.Version);
      Assert.NotNull(_kuzzle.InstanceId);
    }

    [Fact]
    public async void ConnectAsyncTest() {
      await _kuzzle.ConnectAsync();

      _protocol.Mock.Verify(m => m.ConnectAsync(), Times.Once);
    }

    [Fact]
    public void DisconnectTest() {
      _kuzzle.Disconnect();

      _protocol.Mock.Verify(m => m.Disconnect(), Times.Once);
    }


    [Fact]
    public void QueryAsyncTest() {
      JObject request = new JObject {
        { "controller", "test" },
        { "action", "testAction" }
      };
      _kuzzle.AuthenticationToken = "jwt auth token";
      _protocol.SetSendResult(@"{ result: true }");

      _kuzzle.QueryAsync(request);

      _protocol.Mock.Verify(
        protocol => protocol.Send(It.Is<JObject>(o => testSendArg(o))));

      Assert.Single(_kuzzle.requests);
    }
    private bool testSendArg (JObject query) {
      Assert.Equal("test", query["controller"]);
      Assert.Equal("testAction", query["action"]);
      Assert.Equal("jwt auth token", query["jwt"]);
      Assert.True(JToken.DeepEquals(query["volatile"], new JObject {
        { "sdkVersion", _kuzzle.Version },
        { "sdkInstanceId", _kuzzle.InstanceId }
      }));

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
      _kuzzle.TokenExpired += delegate() {
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
      _kuzzle.UnhandledResponse += delegate(object sender, Response response) {
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
    public async void StateChangeListenerTest () {
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
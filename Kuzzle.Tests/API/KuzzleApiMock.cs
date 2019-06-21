using System;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.API;
using KuzzleSdk.Exceptions;
using Moq;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Tests.API {
  public class KuzzleApiMock {
    public Mock<IKuzzleApi> Mock { get; }

    public IKuzzleApi MockedObject {
      get { return Mock.Object; }
    }

    private ApiErrorException GetApiErrorException(int status, string message) {
      var r = Response.FromString($"{{error: {{message: \"{message}\", status: {status} }} }}");
      return new ApiErrorException(r);
    }

    public KuzzleApiMock() {
      Mock = new Mock<IKuzzleApi>();
    }

    public void SetResult(JToken apiResult) {
      SetResult(apiResult.ToString());
    }

    public void SetResult(string apiResult) {
      Mock
        .Setup(api => api.QueryAsync(It.IsAny<JObject>()))
        .Returns(Task.FromResult(Response.FromString(apiResult)));
    }

    public void SetError(int status = 400, string message = "Errored Test") {
      var t = new TaskCompletionSource<Response>();
      t.SetException(GetApiErrorException(status, message));

      Mock
        .Setup(api => api.QueryAsync(It.IsAny<JObject>()))
        .Returns(t.Task);
    }

    public void Verify(JObject expectedQuery) {
      Mock.Verify(
        api => api.QueryAsync(
          It.Is<JObject>(o => JToken.DeepEquals(o, expectedQuery))));
    }
  }
}

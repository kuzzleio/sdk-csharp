using System;
using System.Threading.Tasks;
using Kuzzle.Tests.API;
using Kuzzle.Tests.Protocol;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.Offline;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.Offline {

  public class TokenVerifierTest {

    private TestableOfflineManager testableOfflineManager;
    private TestableWebSocket ws = new TestableWebSocket(new Uri("ws://localhost:1234"));
    private TestableKuzzle kuzzle = new TestableKuzzle();
    private TokenVerifier tokenVerifier;

    public TokenVerifierTest() {
      testableOfflineManager = new TestableOfflineManager(ws, kuzzle);
      tokenVerifier = new TokenVerifier(testableOfflineManager, kuzzle);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task SuccessIsTokenValid(bool isValid, bool needRefresh) {
      kuzzle.AuthenticationToken = "foobar";

      Int64 expiresAt = 
        (Int64)DateTime.UtcNow.AddHours(1.5)
        .Subtract(new DateTime(1970, 1, 1))
        .TotalMilliseconds;

      if (needRefresh) {
        expiresAt =
          (Int64)DateTime.UtcNow.AddHours(0.5)
          .Subtract(new DateTime(1970, 1, 1))
          .TotalMilliseconds;
      }

      kuzzle.mockedAuthController.Setup(obj => 
         obj.CheckTokenAsync(It.IsAny<string>()))
         .Returns(Task.FromResult<JObject>(new JObject {
            { "valid", isValid },
            {"expiresAt", expiresAt}
          })
        );

      bool valid = await tokenVerifier.IsTokenValid();

      kuzzle.mockedAuthController.Verify(
        (obj) => obj.CheckTokenAsync(It.Is<string>(o => o.Equals("foobar")))
      );

      if (needRefresh && isValid) {
        kuzzle.mockedAuthController.Verify(
          (obj) => obj.RefreshTokenAsync(It.IsAny<TimeSpan>()), Times.Once
        );
      } else {
        kuzzle.mockedAuthController.Verify(
          (obj) => obj.RefreshTokenAsync(It.IsAny<TimeSpan>()), Times.Never
        );
      }

      Assert.Equal(isValid, valid);
    }
  }
}

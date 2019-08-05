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
          (obj) => obj.RefreshTokenAsync(It.IsAny<Int64>()), Times.Once
        );
      } else {
        kuzzle.mockedAuthController.Verify(
          (obj) => obj.RefreshTokenAsync(It.IsAny<Int64>()), Times.Never
        );
      }

      Assert.Equal(isValid, valid);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void SuccessDifferentUser(bool autoRecover, bool waitLoginToReplay) {

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.WaitLoginToReplay, waitLoginToReplay);

      tokenVerifier.OnUserLoggedIn(this, new UserLoggedInEvent("foobar"));

      if (waitLoginToReplay && autoRecover) {
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.RejectAllQueries(It.IsAny<Exception>()), Times.Once);
        Assert.False(testableOfflineManager.GetQueryReplayer().WaitLoginToReplay);
        Assert.False(testableOfflineManager.GetQueryReplayer().Lock);
      }
      testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.Clear(), Times.Once);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void SuccessSameUser(bool autoRecover, bool waitLoginToReplay) {

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.WaitLoginToReplay, waitLoginToReplay);

      tokenVerifier.OnUserLoggedIn(this, new UserLoggedInEvent(""));

      if (waitLoginToReplay && autoRecover) {
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Once);
        Assert.False(testableOfflineManager.GetQueryReplayer().WaitLoginToReplay);
        Assert.False(testableOfflineManager.GetQueryReplayer().Lock);
      }
      testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Once);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, true, true)]
    [InlineData(true, false, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    public async Task SuccessCheckTokenToReplay(bool tokenValid, bool autoRecover, bool locked) {

      Int64 expiresAt =
        (Int64)DateTime.UtcNow.AddHours(1.5)
        .Subtract(new DateTime(1970, 1, 1))
        .TotalMilliseconds;

      kuzzle.mockedAuthController.Setup(obj =>
         obj.CheckTokenAsync(It.IsAny<string>()))
         .Returns(Task.FromResult<JObject>(new JObject {
          { "valid", tokenValid },
          { "expiresAt", expiresAt },
         })
       );

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.Lock, locked);

      await tokenVerifier.CheckTokenToReplay();

      if (!autoRecover) {
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Never);
        testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Never);
        return;
      }

      if (!tokenValid) {
        if (locked) {
          testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(It.IsAny<Predicate<JObject>>(), false), Times.Once);
          return;
        }
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Never);
      } else {
          testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Once);
          Assert.False(testableOfflineManager.GetQueryReplayer().Lock);
          testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Once);
      }
    }
  }
}

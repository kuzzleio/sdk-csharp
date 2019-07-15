using System;
using System.Threading.Tasks;
using Kuzzle.Tests.API;
using Kuzzle.Tests.Protocol;
using KuzzleSdk;
using KuzzleSdk.API.Offline;
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
    [InlineData(false)]
    [InlineData(true)]
    public async Task SuccessIsTokenValid(bool isValid) {
      kuzzle.AuthenticationToken = "foobar";

      kuzzle.mockedAuthController.Setup(obj => 
         obj.CheckTokenAsync(It.IsAny<string>()))
         .Returns(Task.FromResult<JObject>(new JObject { { "valid", isValid } }));

      bool valid = await tokenVerifier.IsTokenValid();

      kuzzle.mockedAuthController.Verify(
        (obj) => obj.CheckTokenAsync(It.Is<string>(o => o.Equals("foobar")))
      );

      Assert.Equal(isValid, valid);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async Task SuccessDifferentUser(bool autoRecover, bool waitLoginToReplay) {

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.WaitLoginToReplay, waitLoginToReplay);

      await tokenVerifier.ChangeUser("foobar");

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
    public async Task SuccessSameUser(bool autoRecover, bool waitLoginToReplay) {

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.WaitLoginToReplay, waitLoginToReplay);

      await tokenVerifier.ChangeUser("");

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

      kuzzle.mockedAuthController.Setup(obj =>
         obj.CheckTokenAsync(It.IsAny<string>()))
         .Returns(Task.FromResult<JObject>(new JObject { { "valid", tokenValid } }));

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.Lock, locked);

      await tokenVerifier.CheckTokenToReplay();

      if (!tokenValid) {
        if (autoRecover && locked) {
          testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(It.IsAny<Predicate<JObject>>(), false), Times.Once);
          return;
        }
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Never);
      } else {
        if (autoRecover) {
          testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Once);
          Assert.False(testableOfflineManager.GetQueryReplayer().Lock);
        } else {
          testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Never);
        }
        testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Once);
      }
    }
  }
}

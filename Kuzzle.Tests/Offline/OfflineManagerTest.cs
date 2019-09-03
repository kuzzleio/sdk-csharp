using System;
using System.Threading.Tasks;
using Kuzzle.Tests.API;
using Kuzzle.Tests.Protocol;
using KuzzleSdk;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.Offline;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.Offline {

  public class TestableOfflineManager : OfflineManager {

    public Mock<IQueryReplayer> mockedQueryReplayer;
    public Mock<ISubscriptionRecoverer> mockedSubscriptionRecoverer;
    public Mock<ITokenVerifier> mockedTokenVerifier;

    internal TestableOfflineManager(AbstractProtocol networkProtocol, KuzzleSdk.IKuzzle kuzzle) : base(networkProtocol, kuzzle) {
      MaxQueueSize = -1;
      MaxRequestDelay = 1000;
      MinTokenDuration = 3600000;
      RefreshedTokenDuration = 3600000;
      QueueFilter = null;
    }

    internal override void InitComponents() {
      mockedQueryReplayer = new Mock<IQueryReplayer>();
      mockedSubscriptionRecoverer = new Mock<ISubscriptionRecoverer>();
      mockedTokenVerifier = new Mock<ITokenVerifier>();

      this.QueryReplayer = mockedQueryReplayer.Object;
      this.SubscriptionRecoverer = mockedSubscriptionRecoverer.Object;
      this.TokenVerifier = mockedTokenVerifier.Object;
    }
  }



  public class OfflineManagerTest {
    private OfflineManager offlineManager;
    private TestableWebSocket ws = new TestableWebSocket(new Uri("ws://localhost:1234"));
    private TestableKuzzle kuzzle = new TestableKuzzle();
    private TestableOfflineManager testableOfflineManager;

    public OfflineManagerTest() {
      offlineManager = new OfflineManager(ws, kuzzle);
      offlineManager.MaxQueueSize = -1;
      offlineManager.MaxRequestDelay = 1000;
      offlineManager.MinTokenDuration = 3600000;
      offlineManager.RefreshedTokenDuration = 3600000;
      offlineManager.QueueFilter = null;
      testableOfflineManager = new TestableOfflineManager(ws, kuzzle);
    }




    [Fact]
    public void SuccessGetQueryReplayer() {
      Assert.IsAssignableFrom<IQueryReplayer>(offlineManager.QueryReplayer);
    }

    [Fact]
    public void SuccessGetSubscriptionRecoverer() {
      Assert.IsAssignableFrom<ISubscriptionRecoverer>(offlineManager.SubscriptionRecoverer);
    }

    [Fact]
    public void SuccessGetTokenVerifier() {
      Assert.IsAssignableFrom<ITokenVerifier>(offlineManager.TokenVerifier);
    }

    [Fact]
    public void SuccessGetNetworkProtocol() {
      Assert.IsAssignableFrom<AbstractProtocol>(offlineManager.NetworkProtocol);
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
    public async Task SuccessRecover(bool tokenValid, bool autoRecover, bool locked) {

      Int64 expiresAt =
        (Int64)DateTime.UtcNow.AddHours(1.5)
        .Subtract(new DateTime(1970, 1, 1))
        .TotalMilliseconds;

      testableOfflineManager.mockedTokenVerifier.Setup(obj =>
         obj.IsTokenValid())
         .Returns(
          Task.FromResult<bool>(
            tokenValid
          )
       );

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.Lock, locked);

      await testableOfflineManager.Recover();

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
        Assert.False(testableOfflineManager.QueryReplayer.WaitLoginToReplay);
        testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Once);
      }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void SuccessDifferentUser(bool autoRecover, bool waitLoginToReplay) {

      testableOfflineManager.AutoRecover = autoRecover;
      testableOfflineManager.mockedQueryReplayer.SetupProperty(obj => obj.WaitLoginToReplay, waitLoginToReplay);

      testableOfflineManager.OnUserLoggedIn(this, new UserLoggedInEvent("foobar"));

      if (waitLoginToReplay && autoRecover) {
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.RejectAllQueries(It.IsAny<Exception>()), Times.Once);
        Assert.False(testableOfflineManager.QueryReplayer?.WaitLoginToReplay);
        Assert.False(testableOfflineManager.QueryReplayer?.Lock);
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

      testableOfflineManager.OnUserLoggedIn(this, new UserLoggedInEvent(""));

      if (waitLoginToReplay && autoRecover) {
        testableOfflineManager.mockedQueryReplayer.Verify(obj => obj.ReplayQueries(true), Times.Once);
        Assert.False(testableOfflineManager.QueryReplayer.WaitLoginToReplay);
        Assert.False(testableOfflineManager.QueryReplayer.Lock);
      }
      testableOfflineManager.mockedSubscriptionRecoverer.Verify(obj => obj.RenewSubscriptions(), Times.Once);
    }

  }
}
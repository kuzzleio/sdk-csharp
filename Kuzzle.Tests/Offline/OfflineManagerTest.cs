using System;
using Kuzzle.Tests.API;
using Kuzzle.Tests.Protocol;
using KuzzleSdk;
using KuzzleSdk.API.Offline;
using KuzzleSdk.Offline;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Moq;
using Xunit;

namespace Kuzzle.Tests.Offline {

  public class TestableOfflineManager : OfflineManager {

    public Mock<IQueryReplayer> mockedQueryReplayer;
    public Mock<ISubscriptionRecoverer> mockedSubscriptionRecoverer;
    public Mock<ITokenVerifier> mockedTokenVerifier;

    internal TestableOfflineManager(AbstractProtocol networkProtocol, KuzzleSdk.IKuzzle kuzzle) : base(networkProtocol, kuzzle) {
    }

    internal override void InitComponents() {
      mockedQueryReplayer = new Mock<IQueryReplayer>();
      mockedSubscriptionRecoverer = new Mock<ISubscriptionRecoverer>();
      mockedTokenVerifier = new Mock<ITokenVerifier>();

      this.queryReplayer = mockedQueryReplayer.Object;
      this.subscriptionRecoverer = mockedSubscriptionRecoverer.Object;
      this.tokenVerifier = mockedTokenVerifier.Object;
    }
  }

  public class OfflineManagerTest {
    private OfflineManager offlineManager;
    private TestableWebSocket ws = new TestableWebSocket(new Uri("ws://localhost:1234"));
    private TestableKuzzle kuzzle = new TestableKuzzle();

    public OfflineManagerTest() {
      offlineManager = new OfflineManager(ws, kuzzle);
    }

    [Fact]
    public void SuccessGetQueryReplayer() {
      Assert.IsAssignableFrom<IQueryReplayer>(offlineManager.GetQueryReplayer());
    }

    [Fact]
    public void SuccessGetSubscriptionRecoverer() {
      Assert.IsAssignableFrom<ISubscriptionRecoverer>(offlineManager.GetSubscriptionRecoverer());
    }

    [Fact]
    public void SuccessGetTokenVerifier() {
      Assert.IsAssignableFrom<ITokenVerifier>(offlineManager.GetTokenVerifier());
    }

    [Fact]
    public void SuccessGetNetworkProtocol() {
      Assert.IsAssignableFrom<AbstractProtocol>(offlineManager.GetNetworkProtocol());
    }

  }
}
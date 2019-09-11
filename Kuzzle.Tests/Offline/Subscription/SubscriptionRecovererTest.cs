using System;
using Kuzzle.Tests.API;
using KuzzleSdk;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.Offline.Subscription {
  public class SubscriptionRecovererTest {

    private TestableOfflineManager testableOfflineManager;
    private TestableKuzzle kuzzle = new TestableKuzzle();
    private SubscriptionRecoverer subscriptionRecoverer;
    private Mock<AbstractProtocol> mockedNetworkProtocol;

    public SubscriptionRecovererTest() {
      mockedNetworkProtocol = new Mock<AbstractProtocol>();
      testableOfflineManager = new TestableOfflineManager(mockedNetworkProtocol.Object, kuzzle);
      subscriptionRecoverer = new SubscriptionRecoverer(testableOfflineManager, kuzzle);
    }

    [Fact]
    public void SuccessAdd() {
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foo", "bar", new JObject() { }, null, "id", "channel"));

      Assert.Equal(1, subscriptionRecoverer.Count);
    }

    [Fact]
    public void SuccessRemove() {
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foo", "bar", new JObject() { }, null, "test", "1"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("bar", "foo", new JObject() { }, null, "test", "1"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foobar", "barfoo", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("barfoo", "foobar", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foobar", "foobar", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("barfoo", "barfoo", new JObject() { }, null, "test", "3"));

      Assert.Equal(6, subscriptionRecoverer.Count);

      Assert.Equal(2, subscriptionRecoverer.Remove((obj) => obj.Channel == "1"));

      Assert.Equal(4, subscriptionRecoverer.Count);
    }

    [Fact]
    public void SuccessClear() {
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foo", "bar", new JObject() { }, null, "test", "1"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("bar", "foo", new JObject() { }, null, "test", "1"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foobar", "barfoo", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("barfoo", "foobar", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("foobar", "foobar", new JObject() { }, null, "test", "2"));
      subscriptionRecoverer.Add(new KuzzleSdk.Offline.Subscription.Subscription("barfoo", "barfoo", new JObject() { }, null, "test", "3"));

      Assert.Equal(6, subscriptionRecoverer.Count);

      subscriptionRecoverer.Clear();

      Assert.Equal(0, subscriptionRecoverer.Count);
    }

  }
}

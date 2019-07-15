using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Kuzzle.Tests.API;
using Kuzzle.Tests.Protocol;
using KuzzleSdk;
using KuzzleSdk.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.Offline.Query {
  public class QueryReplayerTest {

    private TestableOfflineManager testableOfflineManager;
    private TestableKuzzle kuzzle = new TestableKuzzle();
    private QueryReplayer queryReplayer;
    private Mock<AbstractProtocol> mockedNetworkProtocol;

    public QueryReplayerTest() {
      mockedNetworkProtocol = new Mock<AbstractProtocol>();
      testableOfflineManager = new TestableOfflineManager(mockedNetworkProtocol.Object, kuzzle);
      queryReplayer = new QueryReplayer(testableOfflineManager, kuzzle.GetKuzzle());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SuccessEnqueue(bool locked) {
      queryReplayer.Lock = locked;
      bool success = queryReplayer.Enqueue(new JObject() { });

      if (locked) {
        Assert.False(success);
        Assert.Equal(0, queryReplayer.Count);
      } else {
        Assert.True(success);
        Assert.False(queryReplayer.Lock);
        Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
        Assert.False(queryReplayer.Lock);
        Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'login'}")));
        Assert.False(queryReplayer.Lock);
        Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'auth', action: 'foobar'}")));
        Assert.False(queryReplayer.Lock);
        Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'auth', action: 'login'}")));
        Assert.True(queryReplayer.Lock);
        Assert.False(queryReplayer.Enqueue(JObject.Parse("{}")));
        Assert.Equal(5, queryReplayer.Count);
      }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(-1)]
    public void SuccessMaxQueueSize(int maxQueueSize) {
      testableOfflineManager.MaxQueueSize = maxQueueSize;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.Equal(maxQueueSize > 2 || maxQueueSize == -1, queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.Equal(maxQueueSize > 2 || maxQueueSize == -1, queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.Equal(maxQueueSize == -1, queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.Equal(maxQueueSize == -1, queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(maxQueueSize == -1 ? 6 : maxQueueSize, queryReplayer.Count);
    }

    [Fact]
    public void SuccessDequeue() {
      testableOfflineManager.MaxQueueSize = -1;
      queryReplayer.Lock = false;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);

      Assert.IsAssignableFrom<JObject>(queryReplayer.Dequeue());
      Assert.Equal(5, queryReplayer.Count);

      queryReplayer.Lock = true;
      Assert.IsAssignableFrom<JObject>(queryReplayer.Dequeue());
      Assert.Equal(4, queryReplayer.Count);

      queryReplayer.Dequeue();
      queryReplayer.Dequeue();
      queryReplayer.Dequeue();
      queryReplayer.Dequeue();
      queryReplayer.Lock = false;
      Assert.Equal(0, queryReplayer.Count);

      Assert.Throws<InvalidOperationException>(() => queryReplayer.Dequeue());
    }

    [Fact]
    public void SuccessRemove() {

      testableOfflineManager.MaxQueueSize = -1;
      queryReplayer.Lock = false;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);
      Assert.Equal(2, queryReplayer.Remove(
      (obj) =>
        obj["controller"]?.ToString() == "foo" ||
        obj["controller"]?.ToString() == "bar"));
      Assert.Equal(4, queryReplayer.Count);

      Assert.Equal(0, queryReplayer.Remove((obj) => obj["action"]?.ToString() == "login"));

      Assert.Equal(4, queryReplayer.Count);
    }

    [Fact]
    public void SuccessClear() {
      testableOfflineManager.MaxQueueSize = -1;
      queryReplayer.Lock = false;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);

      queryReplayer.Clear();

      Assert.Equal(0, queryReplayer.Count);
    }

    [Fact]
    public void SuccessRejectAllQueries() {
      testableOfflineManager.MaxQueueSize = -1;
      queryReplayer.Lock = false;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);

      queryReplayer.RejectAllQueries(new Exception("Some exception"));

      kuzzle.mockedKuzzle.Verify((obj) => obj.GetRequestById(It.IsAny<string>()), Times.Exactly(6));

      Assert.Equal(0, queryReplayer.Count);
    }

    [Fact]
    public void SuccessRejectQueries() {
      testableOfflineManager.MaxQueueSize = -1;
      queryReplayer.Lock = false;

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);

      queryReplayer.RejectQueries(((obj) =>
        obj["controller"]?.ToString() == "foo" || obj["controller"]?.ToString() == "bar"),
        new Exception("Some exception"));

      kuzzle.mockedKuzzle.Verify((obj) => obj.GetRequestById(It.IsAny<string>()), Times.Exactly(2));

      Assert.Equal(4, queryReplayer.Count);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SuccessReplayQueries(bool resetWaitLogin) {
      testableOfflineManager.MaxQueueSize = -1;
      testableOfflineManager.MaxRequestDelay = 0;
      queryReplayer.Lock = false;
      queryReplayer.WaitLoginToReplay = true;
      int callCount = 0;

      queryReplayer.ReplayQuery = (TimedQuery query, CancellationToken token) => {
        callCount += 1;
        return Task.CompletedTask;
      };

      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foo', action: 'bar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'bar', action: 'foor'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'foobar'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'foobar', action: 'barfoo'}")));
      Assert.True(queryReplayer.Enqueue(JObject.Parse("{controller: 'barfoo', action: 'barfoo'}")));

      Assert.Equal(6, queryReplayer.Count);

      Assert.IsType<CancellationTokenSource>(queryReplayer.ReplayQueries(resetWaitLogin));

      if (resetWaitLogin) {
        Assert.False(queryReplayer.WaitLoginToReplay);
      } else {
        Assert.True(queryReplayer.WaitLoginToReplay);
      }

      Assert.Equal(6, callCount);
    }
  }
}


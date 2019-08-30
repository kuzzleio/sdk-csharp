using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.API.Offline;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk {

  public interface IQueryReplayer {
    int Count { get; }
    bool Lock { get; set; }
    bool WaitLoginToReplay { get; set; }

    bool Enqueue(JObject query);
    JObject Dequeue();

    void RejectAllQueries(Exception exception);
    void RejectQueries(Predicate<JObject> predicate, Exception exception);
    int Remove(Predicate<JObject> predicate);

    CancellationTokenSource ReplayQueries(bool resetWaitLogin = true);
    CancellationTokenSource ReplayQueries(Predicate<JObject> predicate, bool resetWaitLogin = true);

    void CancelReplay();
  }

  internal sealed class QueryReplayer : IQueryReplayer {
    private IKuzzle kuzzle;
    private List<TimedQuery> queue;
    private IOfflineManager offlineManager;
    private CancellationTokenSource cancellationTokenSource;
    private bool currentlyReplaying = false;
    private Stopwatch stopWatch = new Stopwatch();

    /// <summary>
    /// Tells if the QueryReplayer is locked (i.e. it doesn't accept new queries).
    /// Should only be true if there is an 'auth:login' or 'auth:logout' call in the queue
    /// </summary>
    public bool Lock { get; set; } = false;

    /// <summary>
    /// True if the QueryReplayer has to wait a login call before replaying the queue
    /// </summary>
    public bool WaitLoginToReplay { get; set; } = false;

    /// <summary>
    /// Constructor of the QueryReplayer class.
    /// </summary>
    internal QueryReplayer(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.queue = new List<TimedQuery>();
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
      this.cancellationTokenSource = new CancellationTokenSource();
      this.ReplayQuery = ReplayOneQuery;
    }

    /// <summary>
    /// Add a new query to the queue.
    /// Return true if successful
    /// </summary>
    public bool Enqueue(JObject query) {
      if (Lock || WaitLoginToReplay) return false;

      lock (queue) {
        if (queue.Count < offlineManager.MaxQueueSize || offlineManager.MaxQueueSize < 0) {
          if (queue.Count == 0) {
            stopWatch.Reset();
            stopWatch.Start();
            queue.Add(new TimedQuery(query, 0));
          } else {
            TimedQuery previous = queue[queue.Count - 1];
            Int64 elapsedTime = stopWatch.ElapsedMilliseconds - previous.Time;
            elapsedTime = Math.Min(elapsedTime, offlineManager.MaxRequestDelay);
            queue.Add(new TimedQuery(query, previous.Time + elapsedTime));
          }
          if (query["controller"]?.ToString() == "auth"
              && (query["action"]?.ToString() == "login"
                || query["action"]?.ToString() == "logout")
              ) {
                Lock = true;
              }

          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Return how many queries are in the queue
    /// </summary>
    public int Count {
      get { return queue.Count; }
    }

    /// <summary>
    /// Remove and return the first query that has been added to the queue.
    /// </summary>
    public JObject Dequeue() {
      lock (queue) {
        if (queue.Count == 0) {
          return null;
        }

        JObject query = queue[0].Query;
        queue.RemoveAt(0);

        return query;
      }
    }

    /// <summary>
    /// Reject every query with an exception.
    /// </summary>
    public void RejectAllQueries(Exception exception) {
      RejectQueries((obj) => true, exception);
    }

    /// <summary>
    /// If the query satisfies the predicate,
    /// it is set with an exception and removed from the replayable queue.
    /// </summary>
    public void RejectQueries(Predicate<JObject> predicate, Exception exception) {
      lock (queue) {
        foreach (TimedQuery timedQuery in queue) {
          if (predicate(timedQuery.Query)) {
            kuzzle.GetRequestById(timedQuery.Query["requestId"]?.ToString())?.SetException(exception);
          }
        }
        queue.RemoveAll((obj) => predicate(obj.Query));
        if (queue.Count == 0) {
          Lock = false;
          currentlyReplaying = false;
          WaitLoginToReplay = false;
        }
      }
    }

    /// <summary>
    /// Remove every query that satisfies the predicate
    /// </summary>
    /// <returns>How many items where removed.</returns>
    public int Remove(Predicate<JObject> predicate) {
      lock (queue) {
        if (queue.Count > 0) {
          Predicate<TimedQuery> timedQueryPredicate = timedQuery => predicate(timedQuery.Query);
          int itemsRemoved = queue.RemoveAll(timedQueryPredicate);
          if (queue.Count == 0 && currentlyReplaying) {
            Lock = false;
            currentlyReplaying = false;
            WaitLoginToReplay = false;
            kuzzle.GetEventHandler().DispatchQueueRecovered();
          }
          return itemsRemoved;
        }
      }
      return 0;
    }

    /// <summary>
    /// Clear the queue.
    /// </summary>
    public void Clear() {
      lock (queue) {
        queue.Clear();
        Lock = false;
        currentlyReplaying = false;
        WaitLoginToReplay = false;
      }
    }

    internal delegate Task ReplayQueryFunc(TimedQuery timedQuery, CancellationToken cancellationToken);

    /// <summary>
    /// Method called to replay one query
    /// </summary>
    internal ReplayQueryFunc ReplayQuery;

    /// <summary>
    /// Replay one query.
    /// </summary>
    internal Task ReplayOneQuery(TimedQuery timedQuery, CancellationToken cancellationToken) {
      if (offlineManager.MaxRequestDelay == 0) {
        if (offlineManager.QueueFilter(timedQuery.Query)) {
          offlineManager.NetworkProtocol.Send(timedQuery.Query);
        }
        return null;
      }

      return Task.Run(async () => {
          if (offlineManager.QueueFilter(timedQuery.Query)) {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay((Int32)timedQuery.Time, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            timedQuery.Query["jwt"] = kuzzle.AuthenticationToken;
            offlineManager.NetworkProtocol.Send(timedQuery.Query);
          }
        }, cancellationToken);
      }

    /// <summary>
    /// Replay the queries with the same elapsed time they were sent.
    /// </summary>
    public CancellationTokenSource ReplayQueries(bool resetWaitLogin = true) {
      return ReplayQueries((obj) => true, resetWaitLogin);
    }

    /// <summary>
    /// Replay the queries with the same elapsed time they were sent if they satisfy the predicate.
    /// </summary>
    public CancellationTokenSource ReplayQueries(Predicate<JObject> predicate, bool resetWaitLogin = true) {
      cancellationTokenSource = new CancellationTokenSource();

      if (resetWaitLogin) WaitLoginToReplay = false;

      lock (queue) {
        if (queue.Count > 0) {
          currentlyReplaying = true;
      
          foreach (TimedQuery timedQuery in queue) {
            if (predicate(timedQuery.Query)) {
              ReplayQuery(timedQuery, cancellationTokenSource.Token);
            }
          }

        }
      }
      return cancellationTokenSource;
    }

    /// <summary>
    /// Cancels the last replay.
    /// </summary>
    public void CancelReplay() {
      this.cancellationTokenSource.Cancel();
    }
  }
}

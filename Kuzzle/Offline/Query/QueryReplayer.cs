using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.API;
using KuzzleSdk.API.Offline;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk {
  /// <summary>
  /// Query replayer interface
  /// </summary>
  public interface IQueryReplayer {
    /// <summary>
    /// Return how many queries are in the queue
    /// </summary>
    int Count { get; }
    /// <summary>
    /// Tells if the QueryReplayer is locked (i.e. it doesn't accept new queries).
    /// Should only be true if there is an 'auth:login' or 'auth:logout' call in the queue
    /// </summary>
    bool Lock { get; set; }
    /// <summary>
    /// True if the QueryReplayer has to wait a login call before replaying the queue
    /// </summary>
    bool WaitLoginToReplay { get; set; }

    /// <summary>
    /// Add a new query to the queue.
    /// Return true if successful
    /// </summary>
    bool Enqueue(JObject query);
    /// <summary>
    /// Remove and return the first query that has been added to the queue.
    /// </summary>
    JObject Dequeue();

    /// <summary>
    /// Reject every query with an exception.
    /// </summary>
    void RejectAllQueries(Exception exception);

    /// <summary>
    /// If the query satisfies the predicate,
    /// it is set with an exception and removed from the replayable queue.
    /// </summary>
    void RejectQueries(Predicate<JObject> predicate, Exception exception);
    /// <summary>
    /// Remove every query that satisfies the predicate
    /// </summary>
    /// <returns>How many items where removed.</returns>
    int Remove(Predicate<JObject> predicate);

    /// <summary>
    /// Replay the queries with the same elapsed time they were sent.
    /// </summary>
    CancellationTokenSource ReplayQueries(bool resetWaitLogin = true);
    /// <summary>
    /// Replay the queries with the same elapsed time they were sent if they satisfy the predicate.
    /// </summary>
    CancellationTokenSource ReplayQueries(Predicate<JObject> predicate, bool resetWaitLogin = true);

    /// <summary>
    /// Cancels the last replay.
    /// </summary>
    void CancelReplay();
  }

  internal sealed class QueryReplayer : IQueryReplayer {
    private IKuzzle kuzzle;
    private List<TimedQuery> queue;
    private IOfflineManager offlineManager;
    private CancellationTokenSource cancellationTokenSource;
    private bool currentlyReplaying = false;
    private Stopwatch stopWatch = new Stopwatch();
    private SemaphoreSlim queueSemaphore = new SemaphoreSlim(1, 1);

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

      queueSemaphore.Wait();
      try {
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
      finally {
        queueSemaphore.Release();
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
      queueSemaphore.Wait();
      try {
        if (queue.Count == 0) {
          return null;
        }

        JObject query = queue[0].Query;
        queue.RemoveAt(0);

        return query;
      }
      finally {
        queueSemaphore.Release();
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
      queueSemaphore.Wait();
      try {
        foreach (TimedQuery timedQuery in queue) {
          if (predicate(timedQuery.Query)) {
            String requestId = timedQuery.Query["requestId"]?.ToString();

            if (requestId != null) {
              TaskCompletionSource<Response> task = kuzzle.GetRequestById(requestId);

              if (task != null) {
                task.SetException(exception);
              }
            }
          }
        }

        queue.RemoveAll((obj) => predicate(obj.Query));

        if (queue.Count == 0) {
          Lock = false;
          currentlyReplaying = false;
          WaitLoginToReplay = false;
        }
      }
      finally {
        queueSemaphore.Release();
      }
    }

    /// <summary>
    /// Remove every query that satisfies the predicate
    /// </summary>
    /// <returns>How many items where removed.</returns>
    public int Remove(Predicate<JObject> predicate) {
      queueSemaphore.Wait();
      try {
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
      finally {
        queueSemaphore.Release();
      }
      return 0;
    }

    /// <summary>
    /// Clear the queue.
    /// </summary>
    public void Clear() {
      queueSemaphore.Wait();
      queue.Clear();
      Lock = false;
      currentlyReplaying = false;
      WaitLoginToReplay = false;
      queueSemaphore.Release();
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

      queueSemaphore.Wait();
      try {
        if (queue.Count > 0) {
          currentlyReplaying = true;

          foreach (TimedQuery timedQuery in queue) {
            if (predicate(timedQuery.Query)) {
              ReplayOneQuery(timedQuery, cancellationTokenSource.Token);
            }
          }

        }
      }
      finally {
        queueSemaphore.Release();
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

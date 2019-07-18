﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;
using KuzzleSdk.Protocol;
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

    private Int64 startTime;
    private IKuzzle kuzzle;
    private List<TimedQuery> queue;
    private IOfflineManager offlineManager;
    private CancellationTokenSource cancellationTokenSource;
    private bool currentlyReplaying = false;

    /// <summary>
    /// Does the QueryReplayer accepts new queries.
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
      queue = new List<TimedQuery>();
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
      this.cancellationTokenSource = new CancellationTokenSource();
      this.ReplayQuery = ReplayOneQuery;
    }

    /// <summary>
    /// Add a new query to the queue.
    /// Return true if success
    /// </summary>
    public bool Enqueue(JObject query) {
      if (Lock) return false;

      lock (queue) {
        if ((queue.Count < offlineManager.MaxQueueSize) || offlineManager.MaxQueueSize < 0) {

          if (queue.Count == 0) {
            startTime = DateTime.Now.Ticks;
            queue.Add(new TimedQuery(query, 0));
          } else {
            TimedQuery previous = queue[queue.Count - 1];
            Int64 elapsedTime = ((DateTime.Now.Ticks - startTime) / 10000) - previous.Time;
            elapsedTime = Math.Min(elapsedTime, offlineManager.MaxRequestDelay);
            queue.Add(new TimedQuery(query, previous.Time + elapsedTime));
          }

          if (query["controller"] != null
              && query["action"] != null
              && query["controller"].ToString() == "auth"
              && (query["action"].ToString() == "login"
              || query["action"].ToString() == "logout")) {
                Lock = true;
            }

          return true;
        }
      }
      return false;
    }

    public int Count {
      get { return queue.Count; }
    }

    /// <summary>
    /// Remove and return the first query that has been added to the queue.
    /// </summary>
    public JObject Dequeue() {
      lock (queue) {
        if (queue.Count == 0)
          throw new InvalidOperationException("Cannot dequeue an Empty queue");
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
    /// If the query satisfy the predicate,
    /// she is set with an exception and removed from the replayable queue.
    /// </summary>
    public void RejectQueries(Predicate<JObject> predicate, Exception exception) {
      lock (queue) {
        for (int i = 0; i < queue.Count; i++) {
          TimedQuery timedQuery = queue[i];
          if (predicate(timedQuery.Query)) {
            kuzzle.GetRequestById(timedQuery.Query["requestId"]?.ToString())?.SetException(exception);
          }
        }
        queue.RemoveAll((obj) => predicate(obj.Query));
      }
    }

    /// <summary>
    /// Remove every query that satisfy the predicate
    /// </summary>
    /// <returns>How many items where removed.</returns>
    public int Remove(Predicate<JObject> predicate) {
      lock (queue) {
        if (queue.Count > 0) {
          Predicate<TimedQuery> timedQueryPredicate = timedQuery => predicate(timedQuery.Query);
          int itemsRemoved = queue.RemoveAll(timedQueryPredicate);
          if (queue.Count == 0 && currentlyReplaying) {
            currentlyReplaying = false;
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
        return Task.Run(async () => {
          if (offlineManager.QueueFilter == null || offlineManager.QueueFilter(timedQuery.Query)) {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay((Int32)timedQuery.Time, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            timedQuery.Query["jwt"] = kuzzle.AuthenticationToken;
            offlineManager.GetNetworkProtocol().Send(timedQuery.Query);
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
      
          for (int i = 0; i < queue.Count; i++) {
            if (predicate(queue[i].Query)) {
              ReplayQuery(queue[i], cancellationTokenSource.Token);
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

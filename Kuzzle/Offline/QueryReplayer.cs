using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk {
  public class QueryReplayer {

    private Int64 startTime;
    private List<TimedQuery> queue;
    private AbstractProtocol networkProtocol;
    private CancellationTokenSource cancellationTokenSource;

    public QueryReplayer(AbstractProtocol networkProtocol) {
      queue = new List<TimedQuery>();
      this.networkProtocol = networkProtocol;
      this.cancellationTokenSource = new CancellationTokenSource();
    }

    public void Enqueue(JObject query) {
      lock (queue) {
        if (queue.Count == 0) {
          startTime = DateTime.Now.Ticks;
          queue.Add(new TimedQuery(query, 0));
        } else {
          TimedQuery previous = queue[queue.Count - 1];
          Int64 elapsedTime = ((DateTime.Now.Ticks - startTime) / 10000) - previous.Time;
          queue.Add(new TimedQuery(query, elapsedTime));
        }
      }
    }

    public JObject Dequeue() {
      lock (queue) {
        if (queue.Count == 0)
          throw new InvalidOperationException("Cannot dequeue an Empty queue");
        JObject query = queue[0].Query;
        queue.RemoveAt(0);

        return query;
      }
    }

    public bool Remove(Predicate<JObject> predicate) {
      if (queue.Count > 0) {
        lock (queue) {
          Predicate<TimedQuery> timedQueryPredicate = timedQuery => predicate(timedQuery.Query);
          if (queue.RemoveAll(timedQueryPredicate) > 0) {
            Console.WriteLine("REMOVE REQUEST");
            return true;
          }
        }
      }
      return false;
    }

    public void Clear() {
      lock (queue) {
        queue.Clear();
      }
    }

    private async Task ReplayQuery(TimedQuery timedQuery, CancellationToken cancellationToken) {
      await Task.Run(async () => {
        cancellationToken.ThrowIfCancellationRequested();
        Console.WriteLine("REPLAY QUERY [" + (string)timedQuery.Query["requestId"] + "]: " + timedQuery.Time);
        await Task.Delay((Int32)timedQuery.Time, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        networkProtocol.Send(timedQuery.Query);
      }, cancellationToken);
    }

    public CancellationTokenSource ReplayQueries() {
      cancellationTokenSource = new CancellationTokenSource();
      lock (queue) {
        queue.Sort();
        for (int i = 0; i < queue.Count; i++) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
          ReplayQuery(queue[i], cancellationTokenSource.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
      }
      return cancellationTokenSource;
    }

    public void CancelLastReplay() {
      this.cancellationTokenSource.Cancel();
    }

  }
}

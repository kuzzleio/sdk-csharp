using System;
using KuzzleSdk.Protocol;

namespace KuzzleSdk.API.Offline {
  public class OfflineManager {


    private int maxQueueSize = -1;
    private int minTokenDuration = 3600000;
    private int maxRequestDelay = 1000;
    private Func<bool> queueFilter = null;
    private bool autoRecover = true;
    private readonly QueryReplayer queryReplayer;


    public int MaxQueueSize {
      get { return maxQueueSize; }
      set { maxQueueSize = value; }
    }

    public int MinTokenDuration {
      get { return minTokenDuration; }
      set { minTokenDuration = value; }
    }

    public int MaxRequestDelay {
      get { return maxRequestDelay; }
      set { maxRequestDelay = value; }
    }

    public Func<bool> QueueFilter {
      get { return queueFilter; }
      set { queueFilter = value; }
    }

    public bool AutoRecover {
      get { return autoRecover; }
      set { autoRecover = value; }
    }

    public OfflineManager(AbstractProtocol networkProtocol) {
      queryReplayer = new QueryReplayer(networkProtocol);
    }

    public QueryReplayer GetQueryReplayer() {
      return queryReplayer;
    }
  }
}

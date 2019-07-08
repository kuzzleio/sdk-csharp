using System;
using System.Threading.Tasks;
using KuzzleSdk.Offline;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Offline {
  public class OfflineManager {

    private ProtocolState previousState = ProtocolState.Closed;
    private int maxQueueSize = -1;
    private int minTokenDuration = 3600000;
    private int maxRequestDelay = 1000;
    private Func<JObject, bool> queueFilter = null;
    private bool autoRecover = true;
    private Kuzzle kuzzle;
    private readonly TokenVerifier tokenVerifier;
    private readonly QueryReplayer queryReplayer;
    private readonly SubscriptionRecoverer subscriptionRecoverer; 
    private readonly AbstractProtocol networkProtocol;

    /// <summary>
    /// The maximum amount of elements that the queue can contains.
    /// If set to -1, the size is unlimited.
    /// </summary>
    public int MaxQueueSize {
      get { return maxQueueSize; }
      set { maxQueueSize = value < 0 ? -1 : value; }
    }

    public int MinTokenDuration {
      get { return minTokenDuration; }
      set { minTokenDuration = value < 0 ? -1 : value; }
    }

    /// <summary>
    /// The maximum delay between two requests to be replayed
    /// </summary>
    public int MaxRequestDelay {
      get { return maxRequestDelay; }
      set { maxRequestDelay = value < 0 ? 0 : value; }
    }

    public Func<JObject, bool> QueueFilter {
      get { return queueFilter; }
      set { queueFilter = value; }
    }

    /// <summary>
    /// Queue requests when network is down, and automatically replay them when the SDK successfully reconnects.
    /// </summary>
    public bool AutoRecover {
      get { return autoRecover; }
      set { autoRecover = value; }
    }

    public OfflineManager(AbstractProtocol networkProtocol, Kuzzle kuzzle) {
      this.networkProtocol = networkProtocol;
      networkProtocol.StateChanged += this.StateChangeListener;
      this.kuzzle = kuzzle;
      queryReplayer = new QueryReplayer(this, kuzzle);
      subscriptionRecoverer = new SubscriptionRecoverer(this, kuzzle.Realtime);
      tokenVerifier = new TokenVerifier(this, kuzzle);
    }

    public AbstractProtocol GetNetworkProtocol() {
      return networkProtocol;
    }

    public QueryReplayer GetQueryReplayer() {
      return queryReplayer;
    }

    public TokenVerifier GetTokenVerifier() {
      return tokenVerifier;
    }

    public SubscriptionRecoverer GetSubscriptionRecoverer() {
      return subscriptionRecoverer;
    }


    private async Task AfterReconnection(bool tokenValid) {
      if (tokenValid) {
        if (AutoRecover) {
          queryReplayer.ReplayQueries();
        }
        subscriptionRecoverer.RenewSubscriptions();
      } else {
         if (queryReplayer.Lock) {
          queryReplayer.RejectQueries((obj) =>
          (obj["controller"] == null
          || obj["action"] == null)
          || obj["controller"].ToString() != "auth"
          || (obj["action"].ToString() != "login" && obj["action"].ToString() != "logout"),
          new KuzzleSdk.Exceptions.NotConnectedException());
          if (AutoRecover) {
            queryReplayer.ReplayQueries();
          }
        } else {
          queryReplayer.RejectQueries((obj) => true, new KuzzleSdk.Exceptions.NotConnectedException());
        }
      }
    }

    internal void StateChangeListener(object sender, ProtocolState state) {
      if (state == ProtocolState.Open && previousState == ProtocolState.Reconnecting) {
        Task.Run(async () => {
          queryReplayer.WaitLoginToReplay = true;
          await tokenVerifier.CheckRefreshToken();
        });
      }
      previousState = state;
    }

  }
}

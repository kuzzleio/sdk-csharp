using System;
using System.Threading.Tasks;
using KuzzleSdk.Offline;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Offline {

  public interface IOfflineManager {
    int MaxQueueSize { get; set; }
    int MinTokenDuration { get; set; }
    int MaxRequestDelay { get; set; }
    Func<JObject, bool> QueueFilter { get; set; }
    bool AutoRecover { get; set; }

    AbstractProtocol GetNetworkProtocol();
    ITokenVerifier GetTokenVerifier();
    ISubscriptionRecoverer GetSubscriptionRecoverer();
    IQueryReplayer GetQueryReplayer();
  }

  public class OfflineManager : IOfflineManager {

    private ProtocolState previousState = ProtocolState.Closed;
    private int maxQueueSize = -1;
    private int minTokenDuration = 3600000;
    private int maxRequestDelay = 1000;
    private Func<JObject, bool> queueFilter = null;
    private bool autoRecover = true;
    private IKuzzle kuzzle;
    protected internal ITokenVerifier tokenVerifier;
    protected internal IQueryReplayer queryReplayer;
    protected internal ISubscriptionRecoverer subscriptionRecoverer;
    protected internal AbstractProtocol networkProtocol;

    /// <summary>
    /// The maximum amount of elements that the queue can contains.
    /// If set to -1, the size is unlimited.
    /// </summary>
    public int MaxQueueSize {
      get { return maxQueueSize; }
      set { maxQueueSize = value < 0 ? -1 : value; }
    }

    /// <summary>
    /// The minimum duration of a Token after refresh.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
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
    /// Queue requests when network is down,
    /// and automatically replay them when the SDK successfully reconnects.
    /// </summary>
    public bool AutoRecover {
      get { return autoRecover; }
      set { autoRecover = value; }
    }

    internal OfflineManager(AbstractProtocol networkProtocol, IKuzzle kuzzle) {
      this.networkProtocol = networkProtocol;
      networkProtocol.StateChanged += this.StateChangeListener;
      this.kuzzle = kuzzle;
      InitComponents();
    }

    internal virtual void InitComponents() {
      queryReplayer = new QueryReplayer(this, kuzzle);
      subscriptionRecoverer = new SubscriptionRecoverer(this, kuzzle.GetRealtime());
      tokenVerifier = new TokenVerifier(this, kuzzle);
    }

    /// <summary>
    /// Gets the network protocol.
    /// </summary>
    /// <returns>The network protocol.</returns>
    public AbstractProtocol GetNetworkProtocol() {
      return networkProtocol;
    }

    /// <summary>
    /// Gets the query replayer.
    /// </summary>
    /// <returns>The query replayer.</returns>
    public IQueryReplayer GetQueryReplayer() {
      return queryReplayer;
    }

    /// <summary>
    /// Gets the token verifier.
    /// </summary>
    /// <returns>The token verifier.</returns>
    public ITokenVerifier GetTokenVerifier() {
      return tokenVerifier;
    }


    /// <summary>
    /// Gets the subscription recoverer.
    /// </summary>
    /// <returns>The subscription recoverer.</returns>
    public ISubscriptionRecoverer GetSubscriptionRecoverer() {
      return subscriptionRecoverer;
    }

    internal void StateChangeListener(object sender, ProtocolState state) {
      if (state == ProtocolState.Open && previousState == ProtocolState.Reconnecting) {
        Task.Run(async () => {
          queryReplayer.WaitLoginToReplay = true;
          await tokenVerifier.CheckTokenToReplay();
        });
      }
      previousState = state;
    }

  }
}

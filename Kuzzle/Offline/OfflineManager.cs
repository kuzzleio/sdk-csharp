using System;
using System.Threading.Tasks;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.Exceptions;
using KuzzleSdk.Offline;
using KuzzleSdk.Offline.Subscription;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Offline {

  public abstract class IOfflineManager {
    public abstract int MaxQueueSize { get; set; }
    public abstract int RefreshedTokenDuration { get; set; }
    public abstract int MinTokenDuration { get; set; }
    public abstract int MaxRequestDelay { get; set; }
    public abstract Func<JObject, bool> QueueFilter { get; set; }
    public abstract bool AutoRecover { get; set; }

    internal abstract AbstractProtocol NetworkProtocol { get; set; }
    internal abstract ITokenVerifier TokenVerifier { get; set; }
    internal abstract ISubscriptionRecoverer SubscriptionRecoverer { get; set; }
    internal abstract IQueryReplayer QueryReplayer { get; set; }

    internal abstract void OnUserLoggedIn(object sender, UserLoggedInEvent e);
    internal abstract Task Recover();

  }

  public class OfflineManager : IOfflineManager {

    private ProtocolState previousState = ProtocolState.Closed;
    private int maxQueueSize;
    private int refreshedTokenDuration;
    private int minTokenDuration;
    private int maxRequestDelay;

    private Func<JObject, bool> queueFilter;
    private IKuzzle kuzzle;

    /// <summary>
    /// The previous user kuid
    /// </summary>
    private string previousKUID = "";

    /// <summary>
    /// The maximum amount of elements that the queue can contains.
    /// If set to -1, the size is unlimited.
    /// </summary>
    public override int MaxQueueSize {
      get { return maxQueueSize; }
      set { maxQueueSize = value < 0 ? -1 : value; }
    }

    /// <summary>
    /// The minimum duration of a Token after refresh.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public override int RefreshedTokenDuration {
      get { return refreshedTokenDuration; }
      set { refreshedTokenDuration = value < 0 ? -1 : value; }
    }

    /// <summary>
    /// The minimum duration of a Token before being automaticaly refreshed.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public override int MinTokenDuration {
      get { return minTokenDuration; }
      set { minTokenDuration = value < 0 ? -1 : value; }
    }

    /// <summary>
    /// The maximum delay between two requests to be replayed
    /// </summary>
    public override int MaxRequestDelay {
      get { return maxRequestDelay; }
      set { maxRequestDelay = value < 0 ? 0 : value; }
    }

    public override Func<JObject, bool> QueueFilter {
      get { return queueFilter; }
      set { queueFilter = value ?? ((obj) => true); }
    }

    /// <summary>
    /// Queue requests when network is down,
    /// and automatically replay them when the SDK successfully reconnects.
    /// </summary>
    public override bool AutoRecover { get; set; } = true;

    internal OfflineManager(AbstractProtocol networkProtocol, IKuzzle kuzzle) {
      this.NetworkProtocol = networkProtocol;
      networkProtocol.StateChanged += this.StateChangeListener;
      this.kuzzle = kuzzle;
      InitComponents();
      this.kuzzle.GetEventHandler().UserLoggedIn += this.OnUserLoggedIn;
    }

    internal virtual void InitComponents() {
      QueryReplayer = new QueryReplayer(this, kuzzle);
      SubscriptionRecoverer = new SubscriptionRecoverer(this, kuzzle);
      TokenVerifier = new TokenVerifier(this, kuzzle);
    }

    /// <summary>
    /// Gets the network protocol.
    /// </summary>
    /// <returns>The network protocol.</returns>
    internal override AbstractProtocol NetworkProtocol { get; set; }

    /// <summary>
    /// Gets the query replayer.
    /// </summary>
    /// <returns>The query replayer.</returns>
    internal override IQueryReplayer QueryReplayer { get; set; }

    /// <summary>
    /// Gets the token verifier.
    /// </summary>
    /// <returns>The token verifier.</returns>
    internal override ITokenVerifier TokenVerifier { get; set; }

    /// <summary>
    /// This will check the token validity,
    /// and chose what to do before replaying the Queue
    /// </summary>
    internal override async Task Recover() {
      if (!AutoRecover) return;

      if (await TokenVerifier.IsTokenValid()) {
        QueryReplayer.ReplayQueries();
        QueryReplayer.Lock = false;
        SubscriptionRecoverer.RenewSubscriptions();
        return;
      }

      if (QueryReplayer.Lock) {
        QueryReplayer.ReplayQueries((obj) =>
        obj["controller"] != null
        && obj["action"] != null
        && obj["controller"].ToString() == "auth"
        && obj["action"].ToString() == "login", false);
      }

    }

    /// <summary>
    /// This is used to verify if the user that has logged in
    /// is the same that before, if not this will Reject every query in the Queue
    /// and clear all subscriptions, otherwise this will replay the Queue if she is waiting.
    /// </summary>
    internal override void OnUserLoggedIn(object sender, UserLoggedInEvent e) {

      if (previousKUID != e.Kuid) {

        if (AutoRecover
           && QueryReplayer.WaitLoginToReplay) {
          QueryReplayer.RejectAllQueries(new UnauthorizeException("Unauthorized", 0));
          QueryReplayer.Lock = false;
          QueryReplayer.WaitLoginToReplay = false;
        }

        SubscriptionRecoverer.Clear();
      } else {

        if (AutoRecover
          && QueryReplayer.WaitLoginToReplay) {
          QueryReplayer.ReplayQueries();
          QueryReplayer.Lock = false;
          QueryReplayer.WaitLoginToReplay = false;
        }

        SubscriptionRecoverer.RenewSubscriptions();
      }
      previousKUID = e.Kuid;
    }

    /// <summary>
    /// Gets the subscription recoverer.
    /// </summary>
    /// <returns>The subscription recoverer.</returns>
    internal override ISubscriptionRecoverer SubscriptionRecoverer { get; set; }

    internal void StateChangeListener(object sender, ProtocolState state) {
      if (state == ProtocolState.Open && previousState == ProtocolState.Reconnecting) {

        kuzzle.GetEventHandler().DispatchReconnected();

        Task.Run(async () => {
          QueryReplayer.WaitLoginToReplay = true;
          await Recover();
        });

      }
      previousState = state;
    }

  }
}

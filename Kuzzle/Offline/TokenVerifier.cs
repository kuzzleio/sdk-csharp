using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.Exceptions;
using KuzzleSdk.Offline.Subscription;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Offline {

  public interface ITokenVerifier {
    Task<bool> IsTokenValid();
    void OnUserLoggedIn(object sender, UserLoggedInEvent e);
    Task CheckTokenToReplay();
  }

  internal sealed class TokenVerifier : ITokenVerifier {

    private readonly ISubscriptionRecoverer subscriptionRecoverer;
    private readonly IOfflineManager offlineManager;
    private readonly IAuthController authController;
    private readonly IQueryReplayer queryReplayer;
    private readonly IKuzzle kuzzle;

    /// <summary>
    /// The previous username logged in
    /// </summary>
    private string previousKUID = "";

    public TokenVerifier(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
      this.authController = kuzzle.GetAuth();

      queryReplayer = offlineManager.GetQueryReplayer();
      subscriptionRecoverer = offlineManager.GetSubscriptionRecoverer();
      kuzzle.GetEventHandler().UserLoggedIn += OnUserLoggedIn;
    }

    /// <summary>
    /// Return true if the Token is valid
    /// </summary>
    public async Task<bool> IsTokenValid() {
      JObject response = await authController.CheckTokenAsync(kuzzle.AuthenticationToken);

      if (response == null || response["valid"] == null) return false;

      bool tokenValid = (bool)response["valid"];



      if (tokenValid
        && offlineManager.RefreshedTokenDuration > -1
        && offlineManager.MinTokenDuration > -1) {

        Int64 remainingTime = (Int64) new DateTime(1970, 1, 1)
          .AddMilliseconds((Int64)response["expiresAt"])
          .Subtract(DateTime.UtcNow)
          .TotalMilliseconds;

        if (offlineManager.MinTokenDuration > remainingTime) {
          try {
            await authController.RefreshTokenAsync(new TimeSpan(offlineManager.MinTokenDuration * 10000));
          } catch (Exception) {

          }
        }
        return true;
      }

      return tokenValid;
    }

    /// <summary>
    /// This is used to verify if the user that has logged in
    /// is the same that before, if not this will Reject every query in the Queue
    /// and clear all subscriptions, otherwise this will replay the Queue if she is waiting.
    /// </summary>
    public void OnUserLoggedIn(object sender, UserLoggedInEvent e) {

      if (previousKUID != e.Kuid) {

        if (offlineManager.AutoRecover && queryReplayer.WaitLoginToReplay) {
            queryReplayer.RejectAllQueries(new UnauthorizeException("Unauthorized", 0));
            queryReplayer.Lock = false;
            queryReplayer.WaitLoginToReplay = false;
        }

        subscriptionRecoverer.Clear();
      } else {

        if (offlineManager.AutoRecover && queryReplayer.WaitLoginToReplay) {
            queryReplayer.ReplayQueries();
            queryReplayer.Lock = false;
            queryReplayer.WaitLoginToReplay = false;
        }

        subscriptionRecoverer.RenewSubscriptions();
      }
      previousKUID = e.Kuid;
    }

    /// <summary>
    /// This will check the token validity,
    /// and chose what to do before replaying the Queue
    /// </summary>
    public async Task CheckTokenToReplay() {
      if (!offlineManager.AutoRecover) return;

      if (await IsTokenValid()) {
          queryReplayer.ReplayQueries();
          queryReplayer.Lock = false;
          subscriptionRecoverer.RenewSubscriptions();
          return;
      }

      if (queryReplayer.Lock) {
          queryReplayer.ReplayQueries((obj) =>
          obj["controller"] != null
          && obj["action"] != null
          && obj["controller"].ToString() == "auth"
          && obj["action"].ToString() == "login", false);
      }

    }

  }
}

﻿using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Offline;
using KuzzleSdk.Offline.Subscription;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Offline {

  public interface ITokenVerifier {
    Task<bool> IsTokenValid();
    Task ChangeUser(string username);
    Task CheckTokenToReplay();
  }

  public class TokenVerifier : ITokenVerifier {
    private IOfflineManager offlineManager;
    private IKuzzle kuzzle;
    private string username = "";
    private readonly IQueryReplayer queryReplayer;
    private readonly ISubscriptionRecoverer subscriptionRecoverer;

    public TokenVerifier(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;

      queryReplayer = offlineManager.GetQueryReplayer();
      subscriptionRecoverer = offlineManager.GetSubscriptionRecoverer();
    }
   
    /// <summary>
    /// Return true if the Token is valid
    /// </summary>
    public async Task<bool> IsTokenValid() {
      JObject response = await kuzzle.GetAuth().CheckTokenAsync(kuzzle.AuthenticationToken);

      if (response != null)
        return (bool)response["valid"];

      return false;
    }

    /// <summary>
    /// This is used to verify if the user that has logged in
    /// is the same that before, if not this will Reject every query in the Queue
    /// and clear all subscriptions, otherwise this will replay the Queue if she is waiting.
    /// </summary>
    public async Task ChangeUser(string username) {

      if (this.username != username) {

        if (offlineManager.AutoRecover && queryReplayer.WaitLoginToReplay) {
            queryReplayer.RejectAllQueries(new KuzzleSdk.Exceptions.ConnectionLostException());
            queryReplayer.Lock = false;
            queryReplayer.WaitLoginToReplay = false;
        }

        subscriptionRecoverer.Clear();
      } else {

        if (queryReplayer.WaitLoginToReplay && offlineManager.AutoRecover) {
            queryReplayer.ReplayQueries();
            queryReplayer.Lock = false;
            queryReplayer.WaitLoginToReplay = false;
        }

        subscriptionRecoverer.RenewSubscriptions();
      }
      this.username = username;
    }

    /// <summary>
    /// This will check the token validity,
    /// and chose what to do before replaying the Queue
    /// </summary>
    public async Task CheckTokenToReplay() {
      if (!(await IsTokenValid())) {
        if (queryReplayer.Lock && offlineManager.AutoRecover) {

            queryReplayer.ReplayQueries((obj) =>
            obj["controller"] != null
            && obj["action"] != null
            && obj["controller"].ToString() == "auth"
            && obj["action"].ToString() == "login", false);

        }
      } else {
        if (offlineManager.AutoRecover) {
          queryReplayer.ReplayQueries();
          queryReplayer.Lock = false;
        }
        offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
      }
    }

  }
}

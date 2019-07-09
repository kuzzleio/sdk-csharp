using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Offline;

namespace KuzzleSdk.Offline {

  public interface ITokenVerifier {
    Task<bool> IsTokenValid();
    Task ChangeUser(string username);
    Task CheckTokenToReplay();
  }

  public class TokenVerifier : ITokenVerifier {
    private OfflineManager offlineManager;
    private Kuzzle kuzzle;
    private string username = "";

    public TokenVerifier(OfflineManager offlineManager, Kuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
    }
   
    /// <summary>
    /// Return true if the Token is valid
    /// </summary>
    public async Task<bool> IsTokenValid() {
      return (bool)(await kuzzle.Auth.CheckTokenAsync(kuzzle.AuthenticationToken))["valid"];
    }

    /// <summary>
    /// This is used to verify if the user that has logged in
    /// is the same that before, if not this will Reject every query in the Queue
    /// and clear all subscriptions, otherwise this will replay the Queue if she is waiting.
    /// </summary>
    public async Task ChangeUser(string username) {
      if (this.username != username) {
      if (offlineManager.AutoRecover) {
          offlineManager.GetQueryReplayer().RejectAllQueries(new KuzzleSdk.Exceptions.ConnectionLostException());
        }
        offlineManager.GetSubscriptionRecoverer().ClearAllSubscriptions();
      } else {
        if (offlineManager.GetQueryReplayer().WaitLoginToReplay) {
          if (offlineManager.AutoRecover) {
            offlineManager.GetQueryReplayer().ReplayQueries();
          }
          offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
        }
      }
      this.username = username;
    }

    /// <summary>
    /// This will check the token validity,
    /// and chose what to do before replaying the Queue
    /// </summary>
    public async Task CheckTokenToReplay() {
      if (!(await IsTokenValid())) {
        if (offlineManager.GetQueryReplayer().Lock) {
          if (offlineManager.AutoRecover) {
            offlineManager.GetQueryReplayer().ReplayQueries((obj) =>
            obj["controller"] != null
            && obj["action"] != null
            && obj["controller"].ToString() == "auth"
            && obj["action"].ToString() == "login", false);
          }
        }
      } else {
        if (offlineManager.AutoRecover) {
          offlineManager.GetQueryReplayer().ReplayQueries();
        }
        offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
      }
    }



  }
}

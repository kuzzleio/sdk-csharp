using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Offline;
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

    public TokenVerifier(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
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
        if (offlineManager.AutoRecover) {
          if (offlineManager.GetQueryReplayer().WaitLoginToReplay) {
            offlineManager.GetQueryReplayer().RejectAllQueries(new KuzzleSdk.Exceptions.ConnectionLostException());
            offlineManager.GetQueryReplayer().Lock = false;
            offlineManager.GetQueryReplayer().WaitLoginToReplay = false;
          }
        }
        offlineManager.GetSubscriptionRecoverer().Clear();
      } else {
        if (offlineManager.GetQueryReplayer().WaitLoginToReplay) {
          if (offlineManager.AutoRecover) {
            offlineManager.GetQueryReplayer().ReplayQueries();
            offlineManager.GetQueryReplayer().Lock = false;
            offlineManager.GetQueryReplayer().WaitLoginToReplay = false;
          }
        }
        offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
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
          offlineManager.GetQueryReplayer().Lock = false;
        }
        offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
      }
    }

  }
}

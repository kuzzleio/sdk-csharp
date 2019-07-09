using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Offline;

namespace KuzzleSdk.Offline {

  public interface ITokenVerifier {
    Task<bool> IsTokenValid();
    Task ChangeUser(string username);
    Task CheckRefreshToken();
  }

  public class TokenVerifier : ITokenVerifier {
    private OfflineManager offlineManager;
    private Kuzzle kuzzle;
    private string username = "";

    public TokenVerifier(OfflineManager offlineManager, Kuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
    }
   
    public async Task<bool> IsTokenValid() {
      return (bool)(await kuzzle.Auth.CheckTokenAsync(kuzzle.AuthenticationToken))["valid"];
    }

    public async Task ChangeUser(string username) {
      if (this.username != username) {
        Console.WriteLine("REJECT ALL QUERIES => USERNAMES NOT MATCHING");
        if (offlineManager.AutoRecover) {
          offlineManager.GetQueryReplayer().RejectAllQueries(new KuzzleSdk.Exceptions.ConnectionLostException());
        }
        offlineManager.GetSubscriptionRecoverer().ClearAllSubscriptions();
      } else {
        if (offlineManager.GetQueryReplayer().WaitLoginToReplay) {
          Console.WriteLine("REPLAY ALL QUERIES => USERNAMES MATCHING");
          if (offlineManager.AutoRecover) {
            offlineManager.GetQueryReplayer().ReplayQueries();
          }
          offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
        }
      }
      this.username = username;
    }

    public async Task CheckRefreshToken() {
      if (!(await IsTokenValid())) {
        if (offlineManager.GetQueryReplayer().Lock) {
          if (offlineManager.AutoRecover) {
            offlineManager.GetQueryReplayer().ReplayQueries((obj) =>
            obj["controller"] != null
            && obj["action"] != null
            && obj["controller"].ToString() == "auth"
            && obj["action"].ToString() == "login", false);
          }
          Console.WriteLine("REPLAY LOGIN");
        } else {
          Console.WriteLine("WAITING FOR LOGIN");
        }
      } else {
        Console.WriteLine("REPLAY ALL QUERIES => EVERYTHING OK");
        if (offlineManager.AutoRecover) {
          offlineManager.GetQueryReplayer().ReplayQueries();
        }
        offlineManager.GetSubscriptionRecoverer().RenewSubscriptions();
      }
    }



  }
}

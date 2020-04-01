using System;
using System.Threading.Tasks;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler.Events;
using KuzzleSdk.Exceptions;
using KuzzleSdk.Offline.Subscription;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Offline {
  /// <summary>
  /// Authentication token verifier
  /// </summary>
  public interface ITokenVerifier {
    /// <summary>
    /// Returns <c>true</c> if the token is valid
    /// </summary>
    Task<bool> IsTokenValid();
  }

  internal sealed class TokenVerifier : ITokenVerifier {

    private readonly IOfflineManager offlineManager;
    private readonly IAuthController authController;
    private readonly IKuzzle kuzzle;

    public TokenVerifier(IOfflineManager offlineManager, IKuzzle kuzzle) {
      this.offlineManager = offlineManager;
      this.kuzzle = kuzzle;
      this.authController = kuzzle.GetAuth();
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
  }
}

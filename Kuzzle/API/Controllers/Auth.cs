using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.API.Controllers {
  public sealed class Auth : Base {
    internal Auth(Kuzzle k) : base(k) { }

    /// <summary>
    /// Checks the validity of an authentication token.
    /// </summary>
    public async Task<JObject> CheckTokenAsync(string token) {
      string jwt = kuzzle.Jwt;
      kuzzle.Jwt = null;
      Response response;

      try {
        response = await kuzzle.Query(new JObject {
          { "controller", "auth" },
          { "action", "checkToken" },
          {
            "body",
            new JObject{
              {"token", token}
            }
          }
        });
      } finally {
        kuzzle.Jwt = jwt;
      }

      return (JObject)response.Result;
    }

    /// <summary>
    /// Creates new credentials for the current user.
    /// </summary>
    public async Task<JObject> CreateMyCredentialsAsync(
        string strategy,
        JObject credentials) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "checkToken" },
        { "body", credentials },
        { "strategy", strategy }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Checks that the current authenticated user has credentials for the 
    /// specified authentication strategy.
    /// </summary>
    public async Task<bool> CredentialsExistAsync(string strategy) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "credentialsExist" },
        { "strategy", strategy }
      });

      return (bool)response.Result;
    }

    /// <summary>
    /// Deletes credentials associated to the current user.
    ///
    /// If the credentials that generated the current authentication token are 
    /// removed, the user will remain logged in until they log out or their 
    /// session expire. After that, they will no longer be able to log in with 
    /// the deleted credentials.
    /// </summary>
    public async Task DeleteMyCredentialsAsync(string strategy) {
      await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "deleteMyCredentials" },
        { "strategy", strategy }
      });
    }

    /// <summary>
    /// Returns information about the currently logged in user.
    /// </summary>
    public async Task<JObject> GetCurrentUserAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "getCurrentUser" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns credentials information for the currently logged in user.
    /// The returned data depends on the given authentication strategy, and 
    /// should never include any sensitive information.
    /// </summary>
    public async Task<JObject> GetMyCredentialsAsync(string strategy) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "getMyCredentials" },
        { "strategy", strategy }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the exhaustive list of granted or denied rights for the 
    /// current user.
    /// </summary>
    public async Task<JArray> GetMyRightsAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "getMyRights" }
      });

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Gets the exhaustive list of registered authentication strategies.
    /// </summary>
    public async Task<JArray> GetStrategiesAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "getStrategies" }
      });

      return (JArray)response.Result;
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    public async Task<JObject> LoginAsync(
        string strategy, JObject credentials, string expiresIn = null) {
      string jwt = kuzzle.Jwt;
      kuzzle.Jwt = null;
      Response response;

      try {
        response = await kuzzle.Query(new JObject {
          { "controller", "auth" },
          { "action", "login" },
          { "strategy", strategy },
          { "body", credentials },
          { "expiresIn", expiresIn }
        });
      } catch (Exception) {
        kuzzle.Jwt = jwt;
        throw;
      }

      kuzzle.Jwt = (string)response.Result["jwt"];

      return (JObject)response.Result;
    }

    /// <summary>
    /// Revokes the provided authentication token.
    /// If there were any, real-time subscriptions are cancelled.
    /// </summary>
    public async Task LogoutAsync() {
      await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "logout" }
      });
    }

    /// <summary>
    /// Refreshes an authentication token.
    /// </summary>
    public async Task<JObject> RefreshTokenAsync(string expiresIn = null) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "login" },
        { "expiresIn", expiresIn }
      });

      kuzzle.Jwt = (string)response.Result["jwt"];

      return (JObject)response.Result;
    }

    /// <summary>
    /// Updates the credentials of the currently logged in user.
    /// </summary>
    public async Task<JObject> UpdateMyCredentialsAsync(
        string strategy,
        JObject credentials) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "updateMyCredentials" },
        { "strategy", strategy },
        { "body", credentials }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Updates the currently logged in user information (the list of 
    /// associated profiles cannot be updated)
    /// </summary>
    public async Task<JObject> UpdateSelfAsync(JObject content) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "updateSelf" },
        { "body", content }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Validates the provided credentials against a specified authentication 
    /// strategy.
    /// This route neither creates nor modifies credentials.
    /// </summary>
    public async Task<bool> ValidateMyCredentialsAsync(
        string strategy,
        JObject credentials) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "auth" },
        { "action", "validateMyCredentials" },
        { "body", credentials },
        { "strategy", strategy }
      });

      return (bool)response.Result;
    }
  }
}

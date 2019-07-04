using System;
using System.Threading.Tasks;
using KuzzleSdk.Utils;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  public class AdminController : BaseController {
    internal AdminController(IKuzzleApi k) : base(k) { }


    /// <summary>
    /// Asynchronously create a snapshot of Kuzzle's state.
    /// Depending on the configuration of Kuzzle.
    /// </summary>
    public async Task<bool> DumpAsync() {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "dump"}
      });

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Load fixtures into the storage layer.
    /// </summary>
    public async Task<bool> LoadFixturesAsync(JObject indexName, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"body", new JObject {
            {"index-name", indexName}
          }
        }
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      Response response = await api.QueryAsync(query);

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Apply mappings to the storage layer.
    /// </summary>
    public async Task<bool> LoadMappingsAsync(JObject indexName, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadMappings"},
        {"body", new JObject {
            {"index-name", indexName}
          }
        }
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      Response response = await api.QueryAsync(query);

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Load roles, profiles and users into the storage layer.
    /// </summary>
    public async Task<bool> LoadSecuritiesAsync(JObject body, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadSecurities"},
        {"body", body}
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      Response response = await api.QueryAsync(query);

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Asynchronously clears the cache database.
    /// </summary>
    public async Task<bool> ResetCacheAsync(string database) {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetCache"},
        {"database", database}
      });

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Asynchronously deletes all indexes created by users.
    /// Neither Kuzzle internal indexes nor Plugin indexes are deleted.
    /// </summary>
    public async Task<bool> ResetDatabaseAsync() {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetDatabase"},
      });

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Asynchronously starts the following sequence, in this order:
    /// - Invalidates and deletes all users along with their associated credentials
    /// - Deletes all user-defined roles and profiles
    /// - Resets the default roles and profiles to their default values
    //Deletes all document validation specifications
    //
    //This action has no impact on Plugin and Document storages.
    /// </summary>
    public async Task<bool> ResetKuzzleDataAsync() {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetKuzzleData"},
      });

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Asynchronously deletes all users, profiles and roles.
    /// Then resets anonymous, default and admin profiles and roles
    /// to default values, specified in Kuzzle configuration files.
    /// </summary>
    public async Task<bool> ResetSecurityAsync() {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetSecurity"},
      });

      return (bool)response.Result["acknowledge"];
    }

    /// <summary>
    /// Safely stops a Kuzzle instance after all remaining requests are processed.
    ///
    /// In a cluster environment, the shutdown action will be propagated across all nodes.
    /// </summary>
    public async Task<bool> ShutdownAsync() {

      Response response = await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "shutdown"},
      });

      return (bool)response.Result["acknowledge"];
    }

  }
}

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
    public async Task DumpAsync() {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "dump"}
      });

    }

    /// <summary>
    /// Load fixtures into the storage layer.
    /// </summary>
    public async Task LoadFixturesAsync(JObject indexName, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"body", new JObject {
            {"index-name", indexName}
          }
        }
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      await api.QueryAsync(query);
    }

    /// <summary>
    /// Apply mappings to the storage layer.
    /// </summary>
    public async Task LoadMappingsAsync(JObject indexName, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadMappings"},
        {"body", new JObject {
            {"index-name", indexName}
          }
        }
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      await api.QueryAsync(query);
    }

    /// <summary>
    /// Load roles, profiles and users into the storage layer.
    /// </summary>
    public async Task LoadSecuritiesAsync(JObject body, bool waitForRefresh = false) {

      JObject query = new JObject {
        {"controller", "admin"},
        {"action", "loadSecurities"},
        {"body", body}
      };

      QueryUtils.HandleRefreshOption(query, waitForRefresh);

      await api.QueryAsync(query);
    }

    /// <summary>
    /// Asynchronously clears the cache database.
    /// </summary>
    public async Task ResetCacheAsync(string database) {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetCache"},
        {"database", database}
      });
    }

    /// <summary>
    /// Asynchronously deletes all indexes created by users.
    /// Neither Kuzzle internal indexes nor Plugin indexes are deleted.
    /// </summary>
    public async Task ResetDatabaseAsync() {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetDatabase"},
      });

    }

    /// <summary>
    /// Asynchronously starts the following sequence, in this order:
    /// - Invalidates and deletes all users along with their associated credentials
    /// - Deletes all user-defined roles and profiles
    /// - Resets the default roles and profiles to their default values
    /// Deletes all document validation specifications
    /// This action has no impact on Plugin and Document storages.
    /// </summary>
    public async Task ResetKuzzleDataAsync() {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetKuzzleData"},
      });

    }

    /// <summary>
    /// Asynchronously deletes all users, profiles and roles.
    /// Then resets anonymous, default and admin profiles and roles
    /// to default values, specified in Kuzzle configuration files.
    /// </summary>
    public async Task ResetSecurityAsync() {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "resetSecurity"},
      });

    }

    /// <summary>
    /// Safely stops a Kuzzle instance after all remaining requests are processed.
    ///
    /// In a cluster environment, the shutdown action will be propagated across all nodes.
    /// </summary>
    public async Task ShutdownAsync() {

      await api.QueryAsync(new JObject {
        {"controller", "admin"},
        {"action", "shutdown"},
      });
    }

  }
}

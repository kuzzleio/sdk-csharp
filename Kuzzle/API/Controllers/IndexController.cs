using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "index" Kuzzle API controller
  /// </summary>
  public class IndexController: BaseController {
    internal IndexController(IKuzzleApi api) : base(api) {}

    /// <summary>
    /// Creates a new index in Kuzzle via the persistence engine.
    /// </summary>
    public async Task<JObject> CreateAsync(string index) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "create" },
        { "index", index },
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Deletes an index from the Kuzzle persistence engine.
    /// </summary>
    public async Task DeleteAsync(string index) {
      await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "delete" },
        { "index", index },
      });
    }

    /// <summary>
    /// Checks if an index exists in the Kuzzle persistence engine.
    /// </summary>
    public async Task<bool> ExistsAsync(string index) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "exists" },
        { "index", index },
      });

      return (bool)response.Result;
    }

    /// <summary>
    /// Gets the autoRefresh flag value for the given index.
    /// </summary>
    public async Task<bool> GetAutoRefreshAsync(string index) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "getAutoRefresh" },
        { "index", index },
      });

      return (bool)response.Result;
    }

    /// <summary>
    /// Lists indexes from the Kuzzle persistence engine.
    /// </summary>
    public async Task<JArray> ListAsync() {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "list" },
      });

      return (JArray)response.Result["indexes"];
    }

    /// <summary>
    /// Deletes multiple indexes from the Kuzzle persistence engine.
    /// </summary>
    public async Task<JArray> MDeleteAsync(JArray indexes) {
      var request = new JObject {
        { "controller", "index" },
        { "action", "mDelete" },
        { "body", new JObject { { "indexes", indexes } } }
      };

      Response response = await api.QueryAsync(request);

      return (JArray)response.Result["indexes"];
    }

    /// <summary>
    /// Forces an immediate reindexation of the provided index.
    /// </summary>
    public async Task<JObject> RefreshAsync(string index) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "refresh" },
        { "index", index },
      });

      return (JObject)response.Result["_shards"];
    }

    /// <summary>
    /// Forces an immediate reindexation of Kuzzle internal storage.
    /// </summary>
    public async Task RefreshInternalAsync() {
      await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "refreshInternal" },
      });
    }

    /// <summary>
    /// Changes the autoRefresh configuration of the given index.
    /// </summary>
    public async Task<bool> SetAutoRefreshAsync(string index, bool autoRefresh) {
      var request = new JObject {
        { "controller", "index" },
        { "action", "setAutoRefresh" }, 
        { "index", index },
        { "body", new JObject { { "autoRefresh", autoRefresh } } }
      };

      Response response = await api.QueryAsync(request);

      return (bool)response.Result["response"];
    }
  }
}

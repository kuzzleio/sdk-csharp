using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Controllers {
  public class ServerController : BaseController {
    public ServerController(Kuzzle k) : base(k) { }

    /// <summary>
    /// Returns the current server timestamp, in Epoch-millis format.
    /// </summary>
    public async Task<Int64> NowAsync() {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "now" }
      });

      return (Int64)response.Result["now"];
    }

    /// <summary>
    /// Returns information about Kuzzle: available API (base + extended),
    /// plugins, external services (Redis, Elasticsearch, ...), servers, etc.
    /// </summary>
    /// <returns>Server information.</returns>
    public async Task<JObject> InfoAsync() {
      ApiResponse response = await kuzzle.Query(new JObject{
        { "controller", "server" },
        { "action", "info" }
      });

      return (JObject)response.Result["serverInfo"];
    }

    /// <summary>
    /// Checks that an administrator account exists.
    /// </summary>
    public async Task<bool> AdminExistsAsync() {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "adminExists" }
      });

      return (bool)response.Result["exists"];
    }

    /// <summary>
    /// Returns all usage statistics
    /// </summary>
    public async Task<JObject> GetAllStatsAsync() {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getAllStats" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the current Kuzzle configuration.
    /// </summary>
    public async Task<JObject> GetConfigAsync() {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getConfig" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the most recent usage statistics snapshot.
    /// </summary>
    public async Task<JObject> GetLastStatsAsync() {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getLastStats" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns usage statistics snapshots within a provided timestamp range. 
    /// </summary>
    public async Task<JObject> GetStatsAsync(Int64 start, Int64 end) {
      ApiResponse response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getLastStats" },
        { "startTime", start},
        { "stopTime", end}
      });

      return (JObject)response.Result;
    }
  }
}

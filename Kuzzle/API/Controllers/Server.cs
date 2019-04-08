using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.API.Controllers {
  public class Server : Base {
    public Server(Kuzzle k) : base(k) { }

    /// <summary>
    /// Returns the current server timestamp, in Epoch-millis format.
    /// </summary>
    public async Task<Int64> NowAsync() {
      Response response = await kuzzle.Query(new JObject {
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
      Response response = await kuzzle.Query(new JObject{
        { "controller", "server" },
        { "action", "info" }
      });

      return (JObject)response.Result["serverInfo"];
    }

    /// <summary>
    /// Checks that an administrator account exists.
    /// </summary>
    public async Task<bool> AdminExistsAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "adminExists" }
      });

      return (bool)response.Result["exists"];
    }

    /// <summary>
    /// Returns all usage statistics
    /// </summary>
    public async Task<JObject> GetAllStatsAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getAllStats" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the current Kuzzle configuration.
    /// </summary>
    public async Task<JObject> GetConfigAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getConfig" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the most recent usage statistics snapshot.
    /// </summary>
    public async Task<JObject> GetLastStatsAsync() {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getLastStats" }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns usage statistics snapshots within a provided timestamp range. 
    /// </summary>
    public async Task<JObject> GetStatsAsync(Int64 start, Int64 end) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "server" },
        { "action", "getLastStats" },
        { "startTime", start},
        { "stopTime", end}
      });

      return (JObject)response.Result;
    }
  }
}

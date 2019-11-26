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
    public async Task CreateAsync(string index) {
      await api.QueryAsync(new JObject {
        { "controller", "index" },
        { "action", "create" },
        { "index", index },
      });
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

      return (JArray)response.Result["deleted"];
    }
  }
}

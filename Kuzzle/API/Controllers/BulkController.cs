using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "bulk" Kuzzle API controller
  /// </summary>
  public sealed class BulkController : BaseController {
    internal BulkController(IKuzzleApi api) : base(api) { }

    /// <summary>
    /// Creates, updates or deletes large amounts of documents as fast as possible.
    /// </summary>
    public async Task<JObject> ImportAsync(
        string index,
        string collection,
        JArray bulkData
    ) {
      Response response = await api.QueryAsync(new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "import"},
        {"body", new JObject {
            {"bulkData", bulkData}
          }
        }
      });
      return (JObject)response.Result;
    }

    /// <summary>
    /// Creates or replaces multiple documents directly into the storage engine.
    /// </summary>
    public async Task<JObject> MWriteAsync(
        string index,
        string collection,
        JArray documents,
        bool waitForRefresh = false,
        bool notify = false
      ) {

      JObject query = new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "mWrite"},
        {"notify", notify},
        {"waitForRefresh", waitForRefresh},
        {"body", new JObject {
            {"documents", documents}
          }
        }
      };

      Response response = await api.QueryAsync(query);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Creates or replaces a document directly into the storage engine.
    /// </summary>
    public async Task<JObject> WriteAsync(
        string index,
        string collection,
        string documentContent,
        string documentId = null,
        bool waitForRefresh = false,
        bool notify = false
      ) {

      JObject query = new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "write"},
        {"_id", documentId},
        {"body", documentContent},
        {"notify", notify},
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(query);

      return (JObject)response.Result;
    }

  }
}

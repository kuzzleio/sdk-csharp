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
    /// Create or replace multiple documents directly into the storage engine.
    /// </summary>
    public async Task<JObject> mWriteAsync(
        string index,
        string collection,
        JArray documents,
        string refresh = null,
        bool? notify = null
      ) {

      Response response = await api.QueryAsync(new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "mWrite"},
        {"notify", notify},
        {"refresh", refresh},
        {"body", new JObject {
            {"documents", documents}
          }
        }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Create or replace a document directly into the storage engine.
    /// </summary>
    public async Task<JObject> WriteAsync(
        string index,
        string collection,
        string documentContent,
        string documentId = null,
        string refresh = null,
        bool? notify = null
      ) {

      Response response = await api.QueryAsync(new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "write"},
        {"_id", documentId},
        {"notify", notify},
        {"refresh", refresh},
        {"body", documentContent}
      });

      return (JObject)response.Result;
    }

  }
}

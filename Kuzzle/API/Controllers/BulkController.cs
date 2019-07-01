using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  public sealed class BulkController : BaseController {
    internal BulkController(IKuzzleApi api) : base(api) { }

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

        {"body", new JObject {
            {"documents", documents}
          }
        }
      });

      return (JObject)response.Result;
    }

    public async Task<JObject> WriteAsync(
        string index,
        string collection,
        string docContent,
        string documentId = null,
        string refresh=null,
        bool? notify = null
      ) {

      Response response = await api.QueryAsync(new JObject {
        {"index", index},
        {"collection", collection},
        {"controller", "bulk"},
        {"action", "mWrite"},
        {"notify", notify},
        {"refresh", refresh},
        {"body", docContent}
      });

      return (JObject)response.Result;
    }

  }
}

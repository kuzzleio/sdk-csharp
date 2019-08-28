using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "collection" Kuzzle API controller
  /// </summary>
  public class CollectionController : BaseController {
    internal CollectionController(IKuzzleApi api) : base(api) { }

    /// <summary>
    /// Creates a new collection in Kuzzle via the persistence engine, in the 
    /// provided index.
    /// </summary>
    public async Task CreateAsync(
        string index,
        string collection,
        JObject mappings = null) {
      await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "create" },
        { "body", mappings },
        { "index", index },
        { "collection", collection }
      });
    }

    /// <summary>
    /// Deletes validation specifications for a collection.
    /// </summary>
    public async Task DeleteSpecificationsAsync(
        string index,
        string collection) {
      await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "deleteSpecifications" },
        { "index", index },
        { "collection", collection }
      });
    }

    /// <summary>
    /// Check if a collection exists in Kuzzle.
    /// </summary>
    public async Task<bool> ExistsAsync(
        string index,
        string collection) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "exists" },
        { "index", index },
        { "collection", collection }
      });

      return (bool)response.Result;
    }

    /// <summary>
    /// Returns a collection mapping.
    /// </summary>
    public async Task<JObject> GetMappingAsync(
        string index,
        string collection) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "getMapping" },
        { "index", index },
        { "collection", collection }
      });

      return (JObject)response.Result[index]["mappings"][collection];
    }

    /// <summary>
    /// Returns the validation specifications associated to the given index and 
    /// collection.
    /// </summary>
    public async Task<JObject> GetSpecificationsAsync(
        string index,
        string collection) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "getSpecifications" },
        { "index", index },
        { "collection", collection }
      });

      return (JObject)response.Result["validation"];
    }

    /// <summary>
    /// Returns the list of collections associated to a provided index. 
    /// The returned list is sorted in alphanumerical order.
    /// </summary>
    public async Task<JObject> ListAsync(
      string index, int? from = null, int? size = null, string type = null
    ) {
      var request = new JObject {
        { "controller", "collection" },
        { "action", "list" },
        { "index", index }
      };

      if (from != null) request.Add("from", from);
      if (size != null) request.Add("size", size);
      if (type != null) request.Add("type", type);

      Response response = await api.QueryAsync(request);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Searches collection specifications.
    /// </summary>
    public async Task<DataObjects.SearchResults> SearchSpecificationsAsync(
        JObject filters,
        Options.SearchOptions options = null) {
      var request = new JObject {
        { "controller", "collection" },
        { "action", "searchSpecifications" },
        { "body", filters }
      };

      if (options != null) {
        request.Merge(JObject.FromObject(options));
      }

      Response response = await api.QueryAsync(request);

      return new DataObjects.SearchResults(api, request, options, response);
    }

    /// <summary>
    /// Empties a collection by removing all its documents, while keeping any 
    /// associated mapping.
    /// </summary>
    public async Task TruncateAsync(
        string index,
        string collection) {
      await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "truncate" },
        { "index", index },
        { "collection", collection }
      });
    }

    /// <summary>
    /// Updates a collection mapping.
    /// </summary>
    public async Task UpdateMappingAsync(
        string index,
        string collection,
        JObject mappings) {
      await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "updateMapping" },
        { "index", index },
        { "collection", collection },
        { "body", mappings }
      });
    }

    /// <summary>
    /// Updates a collection specifications.
    /// </summary>
    public async Task<JObject> UpdateSpecificationsAsync(
        string index,
        string collection,
        JObject specifications) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "updateSpecifications" },
        { "index", index },
        { "collection", collection },
        { "body", specifications }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Validates the provided specifications.
    /// </summary>
    public async Task<bool> ValidateSpecificationsAsync(
        JObject specifications) {
      Response response = await api.QueryAsync(new JObject {
        { "controller", "collection" },
        { "action", "validateSpecifications" },
        { "body", specifications }
      });

      return (bool)response.Result["valid"];
    }
  }
}

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "collection" Kuzzle API controller
  /// </summary>
  public class CollectionController : BaseController {
    internal CollectionController(Kuzzle k) : base(k) { }

    /// <summary>
    /// Creates a new collection in Kuzzle via the persistence engine, in the 
    /// provided index.
    /// </summary>
    public async Task CreateAsync(
        string index,
        string collection,
        JObject mappings = null) {
      await kuzzle.Query(new JObject {
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
      await kuzzle.Query(new JObject {
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
      Response response = await kuzzle.Query(new JObject {
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
      Response response = await kuzzle.Query(new JObject {
        { "controller", "collection" },
        { "action", "getMapping" },
        { "index", index },
        { "collection", collection }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the validation specifications associated to the given index and 
    /// collection.
    /// </summary>
    public async Task<JObject> GetSpecificationsAsync(
        string index,
        string collection) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "collection" },
        { "action", "getSpecifications" },
        { "index", index },
        { "collection", collection }
      });

      return (JObject)response.Result;
    }

    /// <summary>
    /// Returns the list of collections associated to a provided index. 
    /// The returned list is sorted in alphanumerical order.
    /// </summary>
    public async Task<JObject> ListAsync(
        string index,
        Options.ListOptions options = null) {
      var request = new JObject {
        { "controller", "collection" },
        { "action", "list" },
        { "index", index }
      };

      if (options != null) {
        request.Merge(options);
      }

      Response response = await kuzzle.Query(request);

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
        request.Merge(options);
      }

      Response response = await kuzzle.Query(request);

      return new DataObjects.SearchResults(kuzzle, request, options, response);
    }

    /// <summary>
    /// Empties a collection by removing all its documents, while keeping any 
    /// associated mapping.
    /// </summary>
    public async Task TruncateAsync(
        string index,
        string collection) {
      await kuzzle.Query(new JObject {
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
      await kuzzle.Query(new JObject {
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
      Response response = await kuzzle.Query(new JObject {
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
        string index,
        string collection,
        JObject specifications) {
      Response response = await kuzzle.Query(new JObject {
        { "controller", "collection" },
        { "action", "validateSpecifications" },
        { "index", index },
        { "collection", collection },
        { "body", specifications }
      });

      return (bool)response.Result["valid"];
    }
  }
}

using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using KuzzleSdk.API.DataObjects;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "document" Kuzzle API controller
  /// </summary>
  public sealed class DocumentController : BaseController {
    /// <summary>
    /// Sets document options inside the provided JSON object
    /// /!\ MUTATES THE PROVIDED OBJECT /!\
    /// Should only be used on object created internally.
    /// </summary>
    private void ApplyOptions(JObject obj, DocumentOptions opts) {
      if (opts != null) {
        if (opts.WaitForRefresh) {
          obj["refresh"] = "wait_for";
        }

        if (opts.RetryOnConflict != null) {
          obj["retryOnConflict"] = opts.RetryOnConflict;
        }
      }
    }

    internal DocumentController(Kuzzle k) : base(k) { }

    /// <summary>
    /// Counts documents in a collection.
    /// A query can be provided to alter the count result, otherwise returns 
    /// the total number of documents in the collection.
    /// </summary>
    public async Task<int> CountAsync(
        string index,
        string collection,
        JObject query = null) {
      Response response = await kuzzle.QueryAsync(new JObject {
        { "controller", "document" },
        { "action", "count" },
        { "body", query },
        { "index", index },
        { "collection", collection }
      });

      return (int)response.Result["count"];
    }

    /// <summary>
    /// Creates a new document in the persistent data storage.
    /// </summary>
    public async Task<JObject> CreateAsync(
        string index,
        string collection,
        JObject content,
        string id = null,
        DocumentOptions options = null) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "create" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      ApplyOptions(query, options);

      Response response = await kuzzle.QueryAsync(query);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Creates a new document, or replaces its content if it already exists.
    /// </summary>
    public async Task<JObject> CreateOrReplaceAsync(
        string index,
        string collection,
        string id,
        JObject content,
        DocumentOptions options = null) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "createOrReplace" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      ApplyOptions(query, options);

      Response response = await kuzzle.QueryAsync(query);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Deletes a document.
    /// </summary>
    public async Task<string> DeleteAsync(
        string index,
        string collection,
        string id,
        DocumentOptions options = null) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "delete" },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      ApplyOptions(query, options);

      Response response = await kuzzle.QueryAsync(query);

      return (string)response.Result["_id"];
    }

    /// <summary>
    /// Deletes documents matching the provided search query.
    /// An empty or null query will match all documents in the collection.
    /// </summary>
    public async Task<JArray> DeleteByQueryAsync(
        string index,
        string collection,
        JObject query,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "deleteByQuery" },
        { "body", query },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(query, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Gets a document.
    /// </summary>
    public async Task<JObject> GetAsync(
        string index,
        string collection,
        string id) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "get" },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      Response response = await kuzzle.QueryAsync(query);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Creates multiple documents.
    /// Throws a partial error (error code 206) if one or more documents
    /// creations fail.
    /// </summary>
    public async Task<JArray> MCreateAsync(
        string index,
        string collection,
        JArray documents,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mCreate" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Creates or replaces multiple documents.
    /// Throws a partial error (error code 206) if one or more documents
    /// creations/replacements fail.
    /// </summary>
    public async Task<JArray> MCreateOrReplaceAsync(
        string index,
        string collection,
        JArray documents,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mCreateOrReplace" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Deletes multiple documents.
    /// Throws a partial error(error code 206) if one or more document 
    /// deletions fail.
    /// </summary>
    public async Task<JArray> MDeleteAsync(
        string index,
        string collection,
        JArray ids,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mDelete" },
        { "body", new JObject{ { "ids", ids } } },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Gets multiple documents.
    /// Throws a partial error(error code 206) if one or more document can not
    /// be retrieved.
    /// </summary>
    public async Task<JArray> MGetAsync(
        string index,
        string collection,
        JArray ids) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mGet" },
        { "body", new JObject{ { "ids", ids } } },
        { "index", index },
        { "collection", collection }
      };

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Replaces multiple documents.
    /// Throws a partial error(error code 206) if one or more document can not
    /// be replaced.
    /// </summary>
    public async Task<JArray> MReplaceAsync(
        string index,
        string collection,
        JArray documents,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mReplace" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Updates multiple documents.
    /// Throws a partial error(error code 206) if one or more document can not
    /// be replaced.
    /// </summary>
    public async Task<JArray> MUpdateAsync(
        string index,
        string collection,
        JArray documents,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mUpdate" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Replaces the content of an existing document.
    /// </summary>
    public async Task<JObject> ReplaceAsync(
        string index,
        string collection,
        string id,
        JObject content,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "replace" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Searches documents.
    /// </summary>
    public async Task<SearchResults> SeachAsync(
        string index,
        string collection,
        JObject query,
        SearchOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "search" },
        { "body", query },
        { "index", index },
        { "collection", collection }
      };

      if (options != null) {
        request.Merge(options);
      }

      Response response = await kuzzle.QueryAsync(request);
      return new SearchResults(kuzzle, request, options, response);
    }

    /// <summary>
    /// Updates a document content.
    /// </summary>
    public async Task<JObject> UpdateAsync(
        string index,
        string collection,
        string id,
        JObject changes,
        DocumentOptions options = null) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "update" },
        { "body", changes },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      ApplyOptions(request, options);

      Response response = await kuzzle.QueryAsync(request);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Validates data against existing validation rules.
    /// Documents are always valid if no validation rules are defined on 
    /// the provided index and collection.
    /// This request does not store the document.
    /// </summary>
    public async Task<JObject> ValidateAsync(
        string index,
        string collection,
        JObject content) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "validate" },
        { "body", content },
        { "index", index },
        { "collection", collection }
      };

      Response response = await kuzzle.QueryAsync(request);

      return (JObject)response.Result;
    }
  }
}

using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using KuzzleSdk.API.DataObjects;
using Newtonsoft.Json.Linq;
using System;

namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Implements the "document" Kuzzle API controller
  /// </summary>
  public sealed class DocumentController : BaseController {

    internal DocumentController(IKuzzleApi api) : base(api) { }

    /// <summary>
    /// Counts documents in a collection.
    /// A query can be provided to alter the count result, otherwise returns
    /// the total number of documents in the collection.
    /// </summary>
    public async Task<int> CountAsync(
      string index,
      string collection,
      JObject query = null
    ) {
      Response response = await api.QueryAsync(new JObject {
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
      bool waitForRefresh = false
    ) {

      var query = new JObject {
        { "controller", "document" },
        { "action", "create" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(query);

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
      bool waitForRefresh = false
    ) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "createOrReplace" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(query);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Deletes a document.
    /// </summary>
    public async Task<string> DeleteAsync(
      string index,
      string collection,
      string id,
      bool waitForRefresh = false
    ) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "delete" },
        { "index", index },
        { "collection", collection },
        { "_id", id },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(query);

      return (string)response.Result["_id"];
    }

    /// <summary>
    /// Deletes documents matching the provided search query.
    /// An empty or null query will match all documents in the collection.
    /// </summary>
    public async Task<JArray> DeleteByQueryAsync(
      string index,
      string collection,
      JObject query
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "deleteByQuery" },
        { "body", query },
        { "index", index },
        { "collection", collection }
      };

      Response response = await api.QueryAsync(request);

      return (JArray)response.Result["ids"];
    }

    /// <summary>
    /// Gets a document.
    /// </summary>
    public async Task<JObject> GetAsync(
      string index,
      string collection,
      string id
    ) {
      var query = new JObject {
        { "controller", "document" },
        { "action", "get" },
        { "index", index },
        { "collection", collection },
        { "_id", id }
      };

      Response response = await api.QueryAsync(query);

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
      bool waitForRefresh = false
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mCreate" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

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
      bool waitForRefresh = false
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mCreateOrReplace" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

      return (JArray)response.Result["hits"];
    }

    /// <summary>
    /// Deletes multiple documents.
    /// Throws a partial error(error code 206) if one or more document
    /// deletions fail.
    /// </summary>
    public async Task<string[]> MDeleteAsync(
      string index,
      string collection,
      string[] ids,
      bool waitForRefresh = false
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mDelete" },
        { "body", new JObject{ { "ids", new JArray(ids) } } },
        { "index", index },
        { "collection", collection },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

      return response.Result.ToObject<string[]>();
    }

    /// <summary>
    /// Gets multiple documents.
    /// Throws a partial error(error code 206) if one or more document can not
    /// be retrieved.
    /// </summary>
    public async Task<JArray> MGetAsync(
      string index,
      string collection,
      JArray ids
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mGet" },
        { "body", new JObject{ { "ids", ids } } },
        { "index", index },
        { "collection", collection }
      };

      Response response = await api.QueryAsync(request);

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
      bool waitForRefresh = false
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mReplace" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

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
      bool waitForRefresh = false,
      int retryOnConflict = 0
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "mUpdate" },
        { "body", new JObject{ { "documents", documents } } },
        { "index", index },
        { "collection", collection },
        { "retryOnConflict", retryOnConflict },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

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
      bool waitForRefresh = false
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "replace" },
        { "body", content },
        { "index", index },
        { "collection", collection },
        { "_id", id },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Searches documents.
    /// </summary>
    public async Task<SearchResults> SearchAsync(
      string index, string collection, JObject query,
      SearchOptions options = null
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "search" },
        { "body", query },
        { "index", index },
        { "collection", collection }
      };

      if (options != null) {
        request.Merge(JObject.FromObject(options));
      }

      Response response = await api.QueryAsync(request);
      return new SearchResults(api, request, options, response);
    }

    /// <summary>
    /// Updates a document content.
    /// </summary>
    public async Task<JObject> UpdateAsync(
      string index,
      string collection,
      string id,
      JObject changes,
      bool waitForRefresh = false,
      int retryOnConflict = 0
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "update" },
        { "body", changes },
        { "index", index },
        { "collection", collection },
        { "_id", id },
        { "retryOnConflict", retryOnConflict },
        {"waitForRefresh", waitForRefresh},
      };

      Response response = await api.QueryAsync(request);

      return (JObject)response.Result;
    }

    /// <summary>
    /// Validates data against existing validation rules.
    /// Documents are always valid if no validation rules are defined on
    /// the provided index and collection.
    /// This request does not store the document.
    /// </summary>
    public async Task<bool> ValidateAsync(
      string index,
      string collection,
      JObject content
    ) {
      var request = new JObject {
        { "controller", "document" },
        { "action", "validate" },
        { "body", content },
        { "index", index },
        { "collection", collection }
      };

      Response response = await api.QueryAsync(request);

      return (bool)response.Result["valid"];
    }
  }
}

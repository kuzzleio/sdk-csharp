using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.DataObjects {
  /// <summary>
  /// Easy to use wrapper for Kuzzle API search results
  /// </summary>
  public class SearchResults {
    /// <summary>
    /// Kuzzle instance 
    /// </summary>
    protected readonly IKuzzleApi api;

    /// <summary>
    /// Search options
    /// </summary>
    protected readonly SearchOptions options;

    /// <summary>
    /// Search request of origin.
    /// </summary>
    protected readonly JObject request;

    /// <summary>
    /// To be overloaded by specialized SearchResult children
    /// </summary>
    protected readonly string scrollAction = "scroll";

    /// <summary>
    /// Search aggregates
    /// </summary>
    public JObject Aggregations { get; private set; }

    /// <summary>
    /// Search results
    /// </summary>
    public JArray Hits { get; private set; }

    /// <summary>
    /// Total number of found results
    /// </summary>
    public int Total { get; private set; }

    /// <summary>
    /// Number of results fetched so far
    /// </summary>
    public int Fetched { get; private set; }

    /// <summary>
    /// Scroll identifier (if any)
    /// </summary>
    public string ScrollId { get; private set; }

    internal SearchResults(
        IKuzzleApi api, JObject request, SearchOptions options,
        Response response, int previouslyFetched = 0) {
      this.api = api;
      this.options = new SearchOptions(options);
      this.request = (JObject)request.DeepClone();

      Aggregations = (JObject)response.Result["aggregations"];
      Hits = (JArray)response.Result["hits"];
      Total = (int)response.Result["total"];
      Fetched = Hits.Count + previouslyFetched;
      ScrollId = (string)response.Result["scrollId"];
    }

    private JObject GetScrollRequest() {
      return new JObject {
        { "controller", (string)request["controller"] },
        { "action", scrollAction },
        { "scrollId", ScrollId }
      };
    }

    private JObject GetSearchAfterRequest() {
      var nextRequest = request; // "request" is already a deep copy
      JObject lastItem = (JObject)Hits.Last;
      JArray searchAfter = new JArray();

      request["body"]["search_after"] = searchAfter;

      foreach (JToken value in (JArray)request["body"]["sort"]) {
        string key;

        if (value.Type == JTokenType.String) {
          key = (string)value;
        } else {
          key = (string)((JObject)value).First;
        }

        if (key == "_uid") {
          searchAfter.Add((string)request["collection"] + "#"
            + (string)lastItem["_id"]);
        } else {
          searchAfter.Add(lastItem["_source"].SelectToken(key));
        }
      }

      return nextRequest;
    }

    /// <summary>
    /// Returns a new SearchResult object which contain the subsequent results 
    /// of the search.
    /// </summary>
    public async Task<SearchResults> NextAsync() {
      if (Fetched >= Total) return null;

      JObject nextRequest = null;

      if (ScrollId != null) {
        nextRequest = GetScrollRequest();
      } else if (
        options.Size != null &&
        request["body"]["sort"] != null &&
        request["body"]["sort"].Type != JTokenType.Null
      ) {
        nextRequest = GetSearchAfterRequest();
      } else if (options.Size != null) {
        if (options.From != null && options.From > Total) {
          return null;
        }

        options.From = Fetched;
        nextRequest = request;
      }

      if (nextRequest == null) {
        return null;
      }

      nextRequest.Merge(JObject.FromObject(options));

      Response response = await api.QueryAsync(nextRequest);

      return new SearchResults(api, nextRequest, options, response, Fetched);
    }
  }
}

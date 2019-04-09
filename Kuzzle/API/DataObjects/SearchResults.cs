using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.API.DataObjects {
  public class SearchResults {
    protected readonly Kuzzle kuzzle;
    protected readonly SearchOptions options;
    protected readonly JObject request;

    // To be overloaded by specialized SearchResult children
    protected readonly string scrollAction = "scroll";

    public JObject Aggregations { get; private set; }
    public JArray Hits { get; private set; }
    public int Total { get; private set; }
    public int Fetched { get; private set; }
    public string ScrollId { get; private set; }

    internal SearchResults(
        Kuzzle kuzzle, JObject request, SearchOptions options,
        Response response, int previouslyFetched = 0) {
      this.kuzzle = kuzzle;
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
        { "action", scrollAction }
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
      } else if (options.Size != null && request["body"]["sort"] != null) {
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

      nextRequest.Merge(options);

      Response response = await kuzzle.Query(nextRequest);

      return new SearchResults(kuzzle, nextRequest, options, response, Fetched);
    }
  }
}

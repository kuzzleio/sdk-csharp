using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.API.ScrollableResults {
  public class SearchResult {
    protected readonly Kuzzle kuzzle;
    protected readonly SearchOptions options;
    protected readonly JObject sourceApiQuery;

    // To be overloaded by specialized SearchResult children
    protected readonly string scrollAction = "scroll";

    public JObject Aggregations { get; private set; }
    public JArray Hits { get; private set; }
    public int Total { get; private set; }
    public int Fetched { get; private set; }
    public string ScrollId { get; private set; }

    internal SearchResult(
        Kuzzle kuzzle, JObject apiQuery, SearchOptions options,
        ApiResponse response, int previouslyFetched = 0) {
      this.kuzzle = kuzzle;
      this.options = new SearchOptions(options);
      sourceApiQuery = (JObject)apiQuery.DeepClone();

      Aggregations = (JObject)response.Result["aggregations"];
      Hits = (JArray)response.Result["hits"];
      Total = (int)response.Result["total"];
      Fetched = Hits.Count + previouslyFetched;
      ScrollId = (string)response.Result["scrollId"];
    }

    private async Task<SearchResult> NextWithScroll() {
      var query = new JObject {
        { "controller", (string)sourceApiQuery["controller"] },
        { "action", scrollAction }
      };

      query.Merge(options);

      ApiResponse response = await kuzzle.Query(query);

      return new SearchResult(kuzzle, query, options, response, Fetched);
    }

    /// <summary>
    /// Returns a new SearchResult object which contain the subsequent results 
    /// of the search.
    /// </summary>
    public async Task<SearchResult> NextAsync() {
      if (Fetched >= Total) return null;

      if (ScrollId != null) {
        return await NextWithScroll();
      }
      return null;
      //if (options.Size != null && sourceApiQuery["sort"])
    }
  }
}

using Newtonsoft.Json;

namespace Kuzzle.API.ScrollableResults {
  public class SearchOptions {
    [JsonProperty(PropertyName = "from")]
    public int? From { get; set; }

    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

    [JsonProperty(PropertyName = "scroll")]
    public string Scroll { get; set; }

    /// <summary>
    /// Copy constructor. This allows keeping tabs on what search options have
    /// been used while letting users reuse their own SearchOptions instance for
    /// different searches (might otherwise cause inconsistent behaviors)
    /// </summary>
    /// <param name="src">Source.</param>
    internal SearchOptions(SearchOptions src) {
      From = src.From;
      Size = src.Size;
      Scroll = src.Scroll;
    }
  }
}

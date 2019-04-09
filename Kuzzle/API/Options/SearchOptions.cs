using Newtonsoft.Json;
namespace KuzzleSdk.API.Options {
  /// <summary>
  /// Search options.
  /// </summary>
  public class SearchOptions {
    /// <summary>
    /// Pagination start index
    /// </summary>
    [JsonProperty(PropertyName = "from")]
    public int? From { get; set; }

    /// <summary>
    /// Number of hits per result page.
    /// </summary>
    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

    /// <summary>
    /// Scroll duration (setting this value creates a new scroll cursor)
    /// </summary>
    [JsonProperty(PropertyName = "scroll")]
    public string Scroll { get; set; }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    internal SearchOptions(SearchOptions src) {
      From = src.From;
      Size = src.Size;
      Scroll = string.Copy(src.Scroll);
    }
  }
}

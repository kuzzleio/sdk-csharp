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
    public SearchOptions(SearchOptions src) {
      if (src != null) {
        From = src.From;
        Size = src.Size;

        if (src.Scroll != null) {
          Scroll = string.Copy(src.Scroll);
        }
      }
    }

    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="T:KuzzleSdk.API.Options.SearchOptions"/> class.
    /// </summary>
    public SearchOptions() { }
  }
}

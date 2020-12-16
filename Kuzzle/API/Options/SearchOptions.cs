using Newtonsoft.Json;
namespace KuzzleSdk.API.Options {
  /// <summary>
  /// Search options.
  /// </summary>
  public class SearchOptions {
    /// <summary>
    /// Pagination start index
    /// </summary>
    [
      JsonProperty(
        PropertyName = "from",
        NullValueHandling = NullValueHandling.Ignore
      )
    ]
    public int? From { get; set; }

    /// <summary>
    /// Number of hits per result page.
    /// </summary>
    [
      JsonProperty(
        PropertyName = "size",
        NullValueHandling = NullValueHandling.Ignore
      )
    ]
    public int? Size { get; set; }

    /// <summary>
    /// Scroll duration (setting this value creates a new scroll cursor)
    /// </summary>
    [
      JsonProperty(
        PropertyName = "scroll",
        NullValueHandling = NullValueHandling.Ignore
      )
    ]
    public string Scroll { get; set; }

    /// <summary>
    /// Field to sort the result on
    /// </summary>
    [
      JsonProperty(
        PropertyName = "sort",
        NullValueHandling = NullValueHandling.Ignore
      )
    ]
    public string Sort { get; set; }

    /// <summary>
    /// Query Syntax to use
    /// </summary>
    [
      JsonProperty(
        PropertyName = "lang",
        NullValueHandling = NullValueHandling.Ignore
      )
    ]
    public string Lang { get; set; }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    public SearchOptions(SearchOptions src) {
      if (src != null) {
        From = src.From;
        Size = src.Size;

        if (src.Sort != null) {
          Sort = string.Copy(src.Sort);
        }

        if (src.Scroll != null) {
          Scroll = string.Copy(src.Scroll);
        }

        if (src.Lang != null) {
          Lang = string.Copy(src.Lang);
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

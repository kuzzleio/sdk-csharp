using Newtonsoft.Json;

namespace KuzzleSdk.API.Options {
  /// <summary>
  /// List options.
  /// </summary>
  public class ListOptions {

    /// <summary>
    /// Pagination start index
    /// </summary>
    [JsonProperty(PropertyName = "from")]
    public int? From;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    [JsonProperty(PropertyName = "size")]
    public int? Size;

    /// <summary>
    /// Filters the returned items by type.
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public string Type;

    /// <summary>
    /// Copy constructor.
    /// </summary>
    internal ListOptions(ListOptions src) {
      From = src.From;
      Size = src.Size;
      Type = string.Copy(src.Type);
    }
  }
}

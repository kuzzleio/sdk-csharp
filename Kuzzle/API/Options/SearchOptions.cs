using Newtonsoft.Json;
namespace KuzzleSdk.API.Options {
  public class SearchOptions {
    [JsonProperty(PropertyName = "from")]
    public int? From { get; set; }

    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

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

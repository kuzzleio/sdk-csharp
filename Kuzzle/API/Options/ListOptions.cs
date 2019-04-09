using Newtonsoft.Json;

namespace KuzzleSdk.API.Options {
  public class ListOptions {
    [JsonProperty(PropertyName = "from")]
    public int? From;

    [JsonProperty(PropertyName = "size")]
    public int? Size;

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

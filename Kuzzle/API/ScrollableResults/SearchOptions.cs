using System;
namespace Kuzzle.API.ScrollableResults {
  public class SearchOptions {
    public uint? From { get; set; }
    public uint? Size { get; set; }
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

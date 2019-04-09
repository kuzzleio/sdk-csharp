namespace KuzzleSdk.API.Options {
  /// <summary>
  /// Options for the Document controller
  /// </summary>
  public class DocumentOptions {
    /// <summary>
    /// If true, the async task resolves only after the document action has
    /// been fully indexed (default: false)
    /// </summary>
    public bool WaitForRefresh { get; set; } = false;

    /// <summary>
    /// Retries the set number in case of a cluster conflict during a document
    /// update.
    /// </summary>
    public int? RetryOnConflict { get; set; }
  };
}

namespace KuzzleSdk.API.Options {
  public class DocumentOptions {
    public bool WaitForRefresh { get; set; } = false;
    public int? RetryOnConflict { get; set; }
  };
}

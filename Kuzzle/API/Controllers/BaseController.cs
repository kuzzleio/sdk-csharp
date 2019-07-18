namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Base controller.
  /// </summary>
  public class BaseController {
    /// <summary>
    /// Kuzzle instance.
    /// </summary>
    protected readonly IKuzzleApi api;

    internal BaseController(IKuzzleApi k) {
      api = k;
    }
  }
}

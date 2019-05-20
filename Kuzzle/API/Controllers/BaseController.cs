namespace KuzzleSdk.API.Controllers {
  /// <summary>
  /// Base controller.
  /// </summary>
  public class BaseController {
    /// <summary>
    /// Kuzzle instance.
    /// </summary>
    protected readonly Kuzzle kuzzle;

    internal BaseController(Kuzzle k) {
      kuzzle = k;
    }
  }
}

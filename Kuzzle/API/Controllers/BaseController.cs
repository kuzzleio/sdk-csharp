namespace KuzzleSdk.API.Controllers {
  public class BaseController {
    protected readonly Kuzzle kuzzle;

    internal BaseController(Kuzzle k) {
      kuzzle = k;
    }
  }
}

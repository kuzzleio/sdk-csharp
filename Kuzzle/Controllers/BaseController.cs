using System;
namespace Kuzzle.Controllers {
  public class BaseController {
    protected readonly Kuzzle kuzzle;

    public BaseController(Kuzzle k) {
      kuzzle = k;
    }
  }
}

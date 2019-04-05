using System;
namespace Kuzzle.API {
  public class BaseController {
    protected readonly Kuzzle kuzzle;

    public BaseController(Kuzzle k) {
      kuzzle = k;
    }
  }
}

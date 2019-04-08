using System;
namespace Kuzzle.API.Controllers {
  public class Base {
    protected readonly Kuzzle kuzzle;

    public Base(Kuzzle k) {
      kuzzle = k;
    }
  }
}

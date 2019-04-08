namespace Kuzzle.API.Controllers {
  public class Base {
    protected readonly Kuzzle kuzzle;

    internal Base(Kuzzle k) {
      kuzzle = k;
    }
  }
}

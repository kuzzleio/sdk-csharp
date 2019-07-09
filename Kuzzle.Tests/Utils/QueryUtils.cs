using Newtonsoft.Json.Linq;

namespace Kuzzle.Tests.Utils {
  public class QueryUtils {

    public static void HandleRefreshOption(JObject query, bool waitForRefresh) {
      if (waitForRefresh) {
        query.Add("refresh", "wait_for");
      }
    }

    public static void HandleNotifyOption(JObject query, bool notify) {
      if (notify) {
        query.Add("notify", true);
      }
    }

  }
}

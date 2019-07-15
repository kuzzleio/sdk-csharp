using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Utils {
  public class QueryUtils {

    public static void HandleRefreshOption(JObject query, bool waitForRefresh) {
      if (waitForRefresh) {
        query.Add("refresh", "wait_for");
      }
    }

  }
}

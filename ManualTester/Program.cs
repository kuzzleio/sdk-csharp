using System;
using KuzzleSdk;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using KuzzleSdk.API.DataObjects;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, KuzzleSdk.API.Response m) {
      Console.WriteLine(JObject.FromObject(m));
    }

    static public async Task Run() {
      var opts = new KuzzleSdk.Protocol.WebSocketOptions { Ssl = true };
      var ws = new KuzzleSdk.Protocol.WebSocket("uat.explorama.kuzzle.io", opts);
      var kuzzle = new Kuzzle(ws);

      await kuzzle.ConnectAsync();

      try {
        await kuzzle.Auth.LoginAsync("local", new JObject {
          {"username", "eneo"},
          {"password", "eneo-password"}
        }, "1m");
        JObject query = new JObject();
        SearchOptions options = new SearchOptions { Scroll = "1m", Size = 10 };
        SearchResults results = await kuzzle.Document.SearchAsync(
          "public", "missions", query, options);

        do {
          Console.WriteLine("FETCHED = " + results.Fetched);
          results = await results.NextAsync();
        } while (results != null);
      } catch (KuzzleSdk.Exceptions.ApiErrorException e) {
        ;
        Console.WriteLine("API Error code " + e.Status);
        Console.WriteLine("Message: " + e.Message);
      }
    }

    public static void Main(string[] args) {
      Run().Wait();
    }
  }
}

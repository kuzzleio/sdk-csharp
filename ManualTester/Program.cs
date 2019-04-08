using System;
using Kuzzle;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, Response m) {
      Console.WriteLine(JObject.FromObject(m));
    }

    static public async Task Run() {
      var ws = new Kuzzle.Protocol.WebSocket("localhost");
      var kuzzle = new Kuzzle.Kuzzle(ws);

      await kuzzle.ConnectAsync();

      JObject foo = new JObject { { "foo", "bar" } };

      Console.WriteLine(foo["baz"]?["qux"]);

      try {
        //Console.WriteLine("login = " + await kuzzle.Auth.LoginAsync("local",
        //  new JObject { { "username", "foobar" }, { "password", "foobar" } }));
        //Console.WriteLine("current user = " + await kuzzle.Auth.GetCurrentUserAsync());
        Console.WriteLine("documents: " +
          await kuzzle.Document.CreateAsync("foo", "bar",
            new JObject { { "foo", "bar" } }, null));
        ///new Kuzzle.API.DocumentOptions { WaitForRefresh = true }));

        Console.WriteLine("timestamp: " + await kuzzle.Server.NowAsync());
      } catch (Kuzzle.Exceptions.ApiErrorException e) {
        Console.WriteLine("API Error code " + e.Status);
        Console.WriteLine("Message: " + e.Message);
      }
    }

    public static void Main(string[] args) {
      Run().Wait();
    }
  }
}

using System;
using KuzzleSdk;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, KuzzleSdk.API.Response m) {
      Console.WriteLine(JObject.FromObject(m));
    }

    static public async Task Run() {
      var ws = new KuzzleSdk.Protocol.WebSocket("localhost");
      var kuzzle = new Kuzzle(ws);

      await kuzzle.ConnectAsync();

      try {
        //await kuzzle.Realtime.SubscribeAsync(
        //  "foo",
        //  "bar",
        //  new JObject(),
        //  notification => {
        //    Console.WriteLine("NOTIFICATION RECEIVED=");
        //    Console.WriteLine(JObject.FromObject(notification));
        //  }
        //);

        //await kuzzle.Realtime.PublishAsync(
        //  "foo",
        //  "bar",
        //  new JObject { { "foo", "bar" } });

        //await Task.Delay(1000);
        //ws.Disconnect();

        Console.WriteLine("login = " + await kuzzle.Auth.LoginAsync("local",
          new JObject { { "username", "foo" }, { "password", "bar" } }));
        Console.WriteLine("current user = " + await kuzzle.Auth.GetCurrentUserAsync());
        //Console.WriteLine("documents: " +
        //await kuzzle.Document.CreateAsync("foo", "bar",
        //new JObject { { "foo", "bar" } }, null));
        ///new Kuzzle.API.DocumentOptions { WaitForRefresh = true }));

        //Console.WriteLine("timestamp: " + await kuzzle.Server.NowAsync());
      } catch (KuzzleSdk.Exceptions.ApiErrorException e) {
        Console.WriteLine("API Error code " + e.Status);
        Console.WriteLine("Message: " + e.Message);
      }
    }

    public static void Main(string[] args) {
      Run().Wait();
    }
  }
}

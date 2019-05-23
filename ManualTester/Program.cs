using System;
using KuzzleSdk;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk.API.Options;
using KuzzleSdk.API.DataObjects;
using static KuzzleSdk.API.Controllers.RealtimeController;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, KuzzleSdk.API.Response m) {
      Console.WriteLine(JObject.FromObject(m));
    }

    static public async Task Run() {
      //var opts = new KuzzleSdk.Protocol.WebSocketOptions { Ssl = true };
      var ws = new KuzzleSdk.Protocol.WebSocket("localhost");
      var kuzzle = new Kuzzle(ws);

      await kuzzle.ConnectAsync();

      try {
        JArray andFilter = new JArray();
        andFilter.Add(new JObject(new JProperty("equals", new JObject(new JProperty("userId", "foobar")))));
        andFilter.Add(new JObject(new JProperty("equals", new JObject(new JProperty("creationDate", "123456789")))));
        andFilter.Add(new JObject(new JProperty("equals", new JObject(new JProperty("isPlantnetAnalyzed", true)))));

        JObject filters = new JObject(
            new JProperty("and", andFilter)
        );

        Console.WriteLine(filters);

        var listener = new NotificationHandler((notification) => {
          string id = notification.Result.ToString();
          Console.WriteLine("LISTENER : Document " + id);
        });

        SubscribeOptions roomOptions = new SubscribeOptions();
        roomOptions.Scope = "in";

        string room = await kuzzle.Realtime.SubscribeAsync(
          "foo",
          "bar",
          filters,
          listener,
          roomOptions);
        Console.WriteLine("Subscribed");

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

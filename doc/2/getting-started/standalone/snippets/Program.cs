using KuzzleSdk;
using KuzzleSdk.Protocol;
using KuzzleSdk.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace getting_started_csharp
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine(args[0]);

      switch (args[0])
      {
        case "init":
          await Init();
          break;

        case "subscribe":
          await Subscribe();
          break;

        case "create":
          await Create();
          break;
      }
    }

    /* snippet:start:1 */
    static async Task<Kuzzle> GetSdk()
    {
      WebSocket socket = new WebSocket(new Uri("ws://kuzzle:7512"));

      Kuzzle kuzzle = new Kuzzle(socket);

      try {
        await kuzzle.ConnectAsync(CancellationToken.None);
      } catch (KuzzleException e) {
        Console.Error.WriteLine(e.Message);
      }

      return kuzzle;
    }
    /* snippet:end */

    /* snippet:start:2 */
    static async Task Init() {
      Kuzzle kuzzle = await GetSdk();

      try {
        await kuzzle.Index.CreateAsync("nyc-open-data");
        await kuzzle.Collection.CreateAsync("nyc-open-data", "yellow-taxi");
      } catch (KuzzleException e) {
        Console.Error.WriteLine(e.Message);
      }

      Console.WriteLine("nyc-open-data/yellow-taxi ready!");

      return;
    }
    /* snippet:end */

    /* snippet:start:3 */
    static async Task Create() {
      Kuzzle kuzzle = await GetSdk();

      JObject driver = JObject.Parse(@"{
        ""name"": ""Liia"",
        ""birthday"": ""1990-09-12"",
        ""license"": ""B""
      }");

      try {
        await kuzzle.Document.CreateAsync("nyc-open-data", "yellow-taxi", driver);
      } catch (KuzzleException e) {
        Console.Error.WriteLine(e.Message);
      }

      Console.WriteLine("New document successfully created!");

      return;
    }
    /* snippet:end */

    /* snippet:start:4 */
    static async Task Subscribe() {
      CancellationTokenSource token = new CancellationTokenSource();
      Kuzzle kuzzle = await GetSdk();

      try {
        await kuzzle.Realtime.SubscribeAsync(
          "nyc-open-data",
          "yellow-taxi",
          JObject.Parse("{}"),
          (notification) => {
            string name = (string) notification.Result["_source"]["name"];
            string driverId = (string) notification.Result["_id"];

            Console.WriteLine($"New driver {name} with id {driverId} has B license.");

            token.Cancel();
          });
      } catch (KuzzleException e) {
        Console.Error.WriteLine(e.Message);
      }

      Console.WriteLine("Successfully subscribed to document notifications!");

      await Task.Delay(10000, token.Token);

      return;
    }
    /* snippet:end */
  }
}

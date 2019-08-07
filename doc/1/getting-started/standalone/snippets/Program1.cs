using KuzzleSdk;
using KuzzleSdk.Protocol;
using KuzzleSdk.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


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

    static async Task Subscribe() {
      return;
    }
    static async Task Create() {
      return;
    }
  }
}

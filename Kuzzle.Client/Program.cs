using System;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Protocol;

namespace Kuzzle.Client {
  class Program {
    static void Main(string[] args) {
      WebSocket socket = new WebSocket(new Uri("ws://localhost:7512"));
      KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(socket);

      kuzzle.ConnectAsync(CancellationToken.None).Wait();

      Task.Run(async () => {
        while (true) {
          Task.Run(async () => {
            long x = await kuzzle.Server.NowAsync();
            Console.WriteLine(x);
          });
          await Task.Delay(500);
        }
      }).Wait();

    }

  }
}

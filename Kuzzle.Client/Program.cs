using System;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Client {
  class Program {
    static void Main(string[] args) {
      WebSocket socket = new WebSocket(new Uri("ws://localhost:7512"));
      socket.AutoReconnect = true;
      KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(socket);
      kuzzle.Offline.AutoRecover = true;

      kuzzle.ConnectAsync(CancellationToken.None).Wait();

      Task.Run(async () => {
        await kuzzle.Realtime.SubscribeAsync("foo", "bar", new JObject { }, null);
        int i = 0;
        while (true) {
          Task.Run(async () => {
            if (kuzzle.NetworkProtocol.State == ProtocolState.Reconnecting) {
              if (i % 5 == 0) {
                await kuzzle.Auth.LogoutAsync();
              } else {
                long x = await kuzzle.Server.NowAsync();
                Console.WriteLine(x);
              }
            } else {
              long x = await kuzzle.Server.NowAsync();
              Console.WriteLine(x);
            }
            i = i + 1;
          });
          await Task.Delay(500);
        }
      }).Wait();

    }

  }
}

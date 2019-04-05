using System;
using Kuzzle;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, ApiResponse m) {
      Console.WriteLine(JObject.FromObject(m));
    }

    static public async Task Run() {
      var ws = new Kuzzle.Protocol.WebSocket("localhost");
      var kuzzle = new Kuzzle.Kuzzle(ws);

      await kuzzle.ConnectAsync();

      try {
        Console.WriteLine("Timestamp = " + await kuzzle.Server.NowAsync());
        Console.WriteLine("Info = " + await kuzzle.Server.InfoAsync());
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

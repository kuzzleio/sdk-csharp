using System;
using Kuzzle.Protocol;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManualTester {
  class MainClass {
    static public void MessageReceiver(object sender, MessageEventArgs m) {
      Console.WriteLine(m.Message);
    }

    static public async Task Run() {
      var ws = new WebSocket("localhost");

      await ws.ConnectAsync();
      ws.MessageEvent += MessageReceiver;

      await ws.SendAsync(JObject.Parse(@"{controller: 'server', action: 'now'}"));
    }

    public static void Main(string[] args) {
      Run();
      Thread.Sleep(2000);
    }
  }
}

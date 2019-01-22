using Kuzzleio;
using System;

public class FListener : NotificationListener {
  public override void onMessage(NotificationResult res) {
    Console.WriteLine("OK");
  }
}

public class Example {

  static void Main() {
    WebSocket ws = new WebSocket("localhost");
    Kuzzle k = new Kuzzle(ws);

    k.connect();

    FListener cb = new FListener();
    k.realtime.subscribe("index", "collection", "{}", cb);
    k.realtime.publish("index", "collection", "{}");
  }

}
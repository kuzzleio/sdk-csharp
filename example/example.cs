using Kuzzleio;
using System;

public class Example {
  static void Main() {
    WebSocket ws = new WebSocket("localhost");
    Kuzzle k = new Kuzzle(ws);

    k.connect();
    Console.WriteLine(k.server.info());
  }
}
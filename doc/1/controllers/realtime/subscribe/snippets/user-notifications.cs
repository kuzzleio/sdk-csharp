NotificationHandler listener = (notification) => {
  int count = (int)notification.Result["count"];
  Console.WriteLine("Currently " + count + " users in the room");
  Console.WriteLine(notification.Volatile);
  // "{ "username": "nina vkote" }"
};

try {
  // Subscription to notifications when user join or leave
  JObject filters = JObject.Parse("{ exists: 'name' }");
  SubscribeOptions options = new SubscribeOptions();
  options.Users = "all";

  await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    filters,
    listener,
    options);

  // Instantiates a second kuzzle client:
  //  multiple subscriptions made by the same user
  //  will not trigger "new user" notifications
  WebSocket ws2 = new WebSocket(new Uri("ws://kuzzle:7512"));
  Kuzzle kuzzle2 = new Kuzzle(ws2);
  await kuzzle2.ConnectAsync(CancellationToken.None);

  // Sets some volatile data
  SubscribeOptions options2 = new SubscribeOptions();
  options2.Volatile = JObject.Parse("{ username: 'nina vkote' }");

  // Subscribes to the same room with the second client
  await kuzzle2.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    filters,
    listener,
    options2);
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

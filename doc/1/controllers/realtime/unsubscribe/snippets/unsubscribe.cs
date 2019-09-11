NotificationHandler listener = (notification) => {};

try {
  string room_id = await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    new JObject(),
    listener);

  await kuzzle.Realtime.UnsubscribeAsync(room_id);

  Console.WriteLine("Successfully unsubscribed");
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

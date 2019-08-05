NotificationHandler listener = (response) => {};

try {
  string room_id = await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse("{}"),
    listener);

  int count = await kuzzle.Realtime.CountAsync(room_id);

  Console.WriteLine($"Currently {count} active subscription");
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

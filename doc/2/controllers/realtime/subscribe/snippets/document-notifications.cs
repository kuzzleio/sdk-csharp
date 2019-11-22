NotificationHandler listener = (notification) => {
  string id = notification.Result["_id"]?.ToString();

  if (notification.Scope == "in") {
    Console.WriteLine($"Document {id} entered the scope");
  } else {
    Console.WriteLine($"Document {id} left the scope");
  }

};

try {
  // Subscribes to notifications for documents containing a 'name' property
  JObject filters = JObject.Parse("{ exists: 'name' }");
  await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    filters,
    listener);

  // Creates a document matching the provided filters
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse("{ name: 'nina vkote', age: 19 }"),
    "nina-vkote");
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

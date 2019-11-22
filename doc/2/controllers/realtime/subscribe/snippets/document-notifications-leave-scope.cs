NotificationHandler listener = (notification) => {
  string scope = notification.Scope.ToString();
  if (scope == "out") {
    Console.WriteLine("Document left the scope");
  } else {
    Console.WriteLine($"Document moved {scope} the scope");
  }
};

try {
  // Subscribes to notifications when documents leave the scope
  JObject filters = JObject.Parse("{ range: { age: { lte: 20 } } }");
  SubscribeOptions options = new SubscribeOptions();
  options.Scope = "all";

  await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    filters,
    listener,
    options);

  // Creates a document which is in the scope
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse("{ name: 'nina vkote', age: 19 }"),
    "nina-vkote");

  // Updates the document to make it leave the scope
  // we shall receive a notification
  await kuzzle.Document.UpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    "nina-vkote",
    JObject.Parse("{ age: 42 }"));
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

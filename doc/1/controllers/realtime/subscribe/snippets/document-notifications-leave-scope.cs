NotificationHandler listener = (notification) => {
  string scope = notification.Scope.ToString();
  if (scope == "out") {
    Console.WriteLine($"Document moved {scope} from the scope");
  } else {
    Console.WriteLine($"Document moved {scope} the scope");
  }
};

try {
  // Subscribes to notifications when document leaves the scope
  JObject filters = JObject.Parse("{ range: { age: { lte: 20 } } }");
  SubscribeOptions options = new SubscribeOptions();
  options.Scope = "out";

  await kuzzle.Realtime.SubscribeAsync(
    "nyc-open-data",
    "yellow-taxi",
    filters,
    listener,
    options);

  // Creates a document who is in the scope
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse("{ name: 'nina vkote', age: 19 }"),
    "nina-vkote");

  // Update the document so he isn't in the scope anymore
  // we shall receive a notification
  await kuzzle.Document.UpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    "nina-vkote",
    JObject.Parse("{ age: 42 }"));
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

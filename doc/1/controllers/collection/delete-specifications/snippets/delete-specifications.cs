try {
  await kuzzle.Collection.DeleteSpecificationsAsync("nyc-open-data", "yellow-taxi");

  Console.WriteLine("Specifications successfully deleted");
} catch (Exception e) {
  Console.WriteLine(e);
}

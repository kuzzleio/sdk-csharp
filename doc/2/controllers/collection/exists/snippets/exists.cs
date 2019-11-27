try {
  bool exists = await kuzzle.Collection.ExistsAsync("nyc-open-data", "green-taxi");

  if (exists) {
    Console.WriteLine("Collection green-taxi exists");
  }
} catch (Exception e) {
  Console.WriteLine(e);
}

if (await kuzzle.Collection.ExistsAsync("index", "collection")) {
  Console.WriteLine("Exists");
} else {
  Console.WriteLine("Does not exist");
}

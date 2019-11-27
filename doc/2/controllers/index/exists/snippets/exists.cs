if (await kuzzle.Index.ExistsAsync("nyc-open-data")) {
  Console.WriteLine("index exists");
} else {
  Console.WriteLine("index does not exist");
}

try {
  await kuzzle.Collection.TruncateAsync("nyc-open-data", "yellow-taxi");

  Console.WriteLine("Collection successfully truncated");
} catch (Exception e) {
  Console.WriteLine(e);
}

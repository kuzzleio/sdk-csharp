
try {
  JArray deleted = await kuzzle.Document.MDeleteAsync(
    "nyc-open-data",
    "yellow-taxi",
    JArray.Parse(@"[""some-id"", ""some-other-id""]"));

  Console.WriteLine("Successfully deleted " + deleted.Count + " documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}


try {
  JObject response = await kuzzle.Document.MDeleteAsync(
    "nyc-open-data",
    "yellow-taxi",
    new string[] { "some-id", "some-other-id" });

  Console.WriteLine($"Successfully deleted {((JArray)response["successes"]).Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

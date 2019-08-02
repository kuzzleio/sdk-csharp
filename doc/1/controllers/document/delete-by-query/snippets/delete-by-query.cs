try {
  JArray hits =
    await kuzzle.Document.DeleteByQueryAsync(
      "nyc-open-data",
      "yellow-taxi",
      JObject.Parse(@"{
        ""query"": {
          ""term"": {
            ""capacity"": 7
          }
        }
      }"));

  Console.WriteLine("Successfully deleted " + hits.Count + " documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e.Message);
}

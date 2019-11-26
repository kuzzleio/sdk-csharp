try {
  for (int i = 0; i < 5; i++) {
    await kuzzle.Document.CreateAsync(
      "nyc-open-data",
      "yellow-taxi",
      JObject.Parse(@"{
        ""category"": ""suv""
      }"));
  }
  for (int i = 5; i < 15; i++) {
    await kuzzle.Document.CreateAsync(
      "nyc-open-data",
      "yellow-taxi",
      JObject.Parse(@"{
        ""category"": ""limousine""
      }"));
  }
  await kuzzle.Collection.RefreshAsync("nyc-open-data", "yellow-taxi");

  SearchOptions options = new SearchOptions();
  options.From = 0;
  options.Size = 2;

  SearchResult result = await kuzzle.Document.SearchAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{
      ""query"": {
        ""match"": {
          ""category"": ""suv""
        }
      }
    }"),
    options);

  Console.WriteLine($"Successfully retrieved {result.Total} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

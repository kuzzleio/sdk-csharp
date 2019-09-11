try {
  SearchOptions options = new SearchOptions();
  options.From = 0;
  options.Size = 2;

  SearchResults response = await kuzzle.Collection.SearchSpecificationsAsync(
    JObject.Parse(@"{
      query: {
        match_all: {}
      }
    }"),
    options);

  Console.WriteLine($"Successfully retrieved {response.Fetched} specifications");
} catch (Exception e) {
  Console.WriteLine(e);
}

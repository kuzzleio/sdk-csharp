JArray documents = new JArray();

for (int i = 0; i < 100; i++) {
  JObject documentBody = new JObject {
    { "category", "suv" }
  };

  documents.Add(
    new JObject {
      { "_id", $"suv_no{i}" },
      { "body", documentBody }
    }
  );
}

await kuzzle.Document.MCreateAsync(
  "nyc-open-data",
  "yellow-taxi",
  documents,
  waitForRefresh: true);

SearchOptions options = new SearchOptions();
options.From = 1;
options.Size = 5;
SearchResult results = await kuzzle.Document.SearchAsync(
  "nyc-open-data",
  "yellow-taxi",
  JObject.Parse(@"{ query: { match: { category: 'suv' } } }"),
  options);

// Fetch the matched items by advancing through the result pages
JArray matched = new JArray();

while (results != null) {
  matched.Merge(results.Hits);
  results = await results.NextAsync();
}

Console.WriteLine(matched[0]);
/*
  { _id: 'suv_no1',
    _score: 0.03390155,
    _source:
      { _kuzzle_info:
        { author: '-1',
          updater: null,
          updatedAt: null,
          createdAt: 1570093133057 },
        category: 'suv' } }
*/
Console.WriteLine($"Successfully retrieved {matched.Count} documents")

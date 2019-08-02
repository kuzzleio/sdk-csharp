try {
  JArray documents = JArray.Parse(@"
    [
      {
        ""_id"": ""some-id"",
        ""body"": {""category"": ""sedan""}
      },
      {
        ""_id"": ""some-other-id"",
        ""body"": {""category"": ""limousine""}
      }
    ]
  ");

  JArray response = await kuzzle.Document.MUpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
    /*
      [ { _id: 'some-id',
        _source:
          { _kuzzle_info:
            { active: true,
              author: '-1',
              updater: null,
              updatedAt: null,
              deletedAt: null,
              createdAt: 1538639586995 },
            capacity: 4,
            category: "sedan"},
        _index: 'nyc-open-data',
        _type: 'yellow-taxi',
        _version: 2,
        result: 'updated',
        _shards: { total: 2, successful: 1, failed: 0 },
        created: false,
        status: 200 },
      { _id: 'some-other-id',
        _source:
          { _kuzzle_info:
            { active: true,
              author: '-1',
              updater: null,
              updatedAt: null,
              deletedAt: null,
              createdAt: 1538639586995 },
            capacity: 4,
            category: "limousine" },
        _index: 'nyc-open-data',
        _type: 'yellow-taxi',
        _version: 2,
        result: 'updated',
        _shards: { total: 2, successful: 1, failed: 0 },
        created: false,
        status: 200 } ]
    */
  Console.WriteLine("Successfully updated " + response.Count + " documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

try {
  JArray documents = JArray.Parse(@"
    [
      {
        ""_id"": ""some-id"",
        ""body"": {""capacity"": 4}
      },
      {
        ""_id"": ""some-other-id"",
        ""body"": {""capacity"": 4}
      }
    ]
  ");

  JObject response = await kuzzle.Document.MReplaceAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
    /*
    { successes:
      [ { _id: 'some-id',
          _version: 2,
          _source: {
            _kuzzle_info:
            { author: '-1',
              updater: null,
              updatedAt: null,
              createdAt: 1538639586995 },
            capacity: 4 }
      },
      [ { _id: 'some-other-id',
          _version: 2,
          _source: {
            _kuzzle_info:
            { author: '-1',
              updater: null,
              updatedAt: null,
              createdAt: 1538639586995 },
            capacity: 4 } ],
      errors: [] }
  */

  Console.WriteLine($"Successfully replaced {((JArray)response["successes"]).Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

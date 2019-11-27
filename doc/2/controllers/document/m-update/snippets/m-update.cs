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

  JObject response = await kuzzle.Document.MUpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
    /*
    { successes:
      [ { _id: 'some-id',
          _source: { _kuzzle_info: [Object], category: 'sedan' },
          _version: 2,
          result: 'updated',
          created: false,
          status: 200 },
        { _id: 'some-other-id',
          _source: { _kuzzle_info: [Object], category: 'limousine' },
          _version: 2,
          result: 'updated',
          created: false,
          status: 200 } ],
    errors: [] }
    */
  Console.WriteLine($"Successfully updated {((JArray)response["successes"]).Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

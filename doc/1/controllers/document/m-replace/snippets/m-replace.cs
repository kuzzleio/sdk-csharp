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

  JArray response = await kuzzle.Document.MReplaceAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
    /*
      {  hits:
        [ { _id: 'some-id',
          _source:
            { _kuzzle_info:
              { active: true,
                author: '-1',
                updater: null,
                updatedAt: null,
                deletedAt: null,
                createdAt: 1538639586995 },
              capacity: 4 },
          _version: 2,
          result: 'replaced',
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
              capacity: 4 },
          _version: 2,
          result: 'replaced',
          status: 200 } ],
      total: 2 }
  */

  Console.WriteLine("Successfully replaced " + response.Count + " documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e.Message);
}

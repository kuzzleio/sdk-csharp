JArray documents = JArray.Parse(@"[
  {
    ""_id"": ""some-id"",
    ""body"": { ""capacity"": 4 }
  },
  {
    ""_id"": ""some-other-id"",
    ""body"": { ""capacity"": 4 }
  }
]");

try {
  JArray response = await kuzzle.Document.MCreateOrReplaceAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
  /*
  [
    {
      "_id":"some-id",
      "_source":{
        "_kuzzle_info":{
          "active":true,
          "author":"-1",
          "updater":null,
          "updatedAt":null,
          "deletedAt":null,
          "createdAt":1538552685790
        },
        "capacity":4
      },
      "_version":1,
      "result":"created",
      "status":201
    },
    {
      "_id":"some-other-id",
      "_source":{
        "_kuzzle_info":{
          "active":true,
          "author":"-1",
          "updater":null,
          "updatedAt":null,
          "deletedAt":null,
          "createdAt":1538552685790
        },
        "capacity":4
      },
      "_version":1,
      "result":"created",
      "status":201
    }
  ]
  */
  Console.WriteLine($"Successfully created {response.Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

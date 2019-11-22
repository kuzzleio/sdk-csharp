JArray documents = JArray.Parse(@"[
  {
    ""_id"": ""some-id"",
    ""body"": { ""capacity"": 4 }
  },
  {
    ""body"": { ""this"": ""document id is auto-computed"" }
  }
]");

try {
  JArray response = await kuzzle.Document.MCreateAsync(
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
            "createdAt":1538470871764
          },
          "capacity":4
      },
      "_version":1,
      "result":"created",
      "status":201
    },
    {
      "_id":"AWY0AoLgKWETYfLdcMat",
      "_source":{
          "_kuzzle_info":{
            "active":true,
            "author":"-1",
            "updater":null,
            "updatedAt":null,
            "deletedAt":null,
            "createdAt":1538470871764
          },
          "this":"document id is auto-computed"
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

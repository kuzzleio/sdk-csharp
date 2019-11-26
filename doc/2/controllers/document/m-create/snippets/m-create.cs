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
  JObject response = await kuzzle.Document.MCreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    documents);

  Console.WriteLine(response.ToString());
  /*
  {
    "successes": [
      {
        "_id":"some-id",
        "_source":{
            "_kuzzle_info":{
              "active":true,
              "author":"-1",
              "updater":null,
              "updatedAt":null
            },
            "capacity":4
        },
        "_version":1
      },
      {
        "_id":"AWY0AoLgKWETYfLdcMat",
        "_source":{
            "_kuzzle_info":{
              "active":true,
              "author":"-1",
              "updater":null,
              "updatedAt":null
            },
            "this":"document id is auto-computed"
        },
        "_version":1
      }
    ],
    "errors": []
  }
  */

  Console.WriteLine($"Successfully created {((JArray)response["successes"]).Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

try {
  JObject response = await kuzzle.Document.MGetAsync(
    "nyc-open-data",
    "yellow-taxi",
    JArray.Parse(@"[""some-id"", ""some-other-id""]"));

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
        "_id":"some-other-id",
        "_source":{
            "_kuzzle_info":{
              "active":true,
              "author":"-1",
              "updater":null,
              "updatedAt":null
            },
            "capacity":"7"
        },
        "_version":1
      }
    ],
    "errors": []
  }
  */
  Console.WriteLine($"Successfully retrieved {((JArray)response["successes"]).Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

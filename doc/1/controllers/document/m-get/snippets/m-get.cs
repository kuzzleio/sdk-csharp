try {
  JArray response = await kuzzle.Document.MGetAsync(
    "nyc-open-data",
    "yellow-taxi",
    JArray.Parse(@"[""some-id"", ""some-other-id""]"));

  Console.WriteLine(response.ToString());
  /*
    [
      {
        "_index": "nyc-open-data",
        "_type": "yellow-taxi",
        "_id": "some-id",
        "_version": 1,
        "found": true,
        "_source": {
          "capacity": 4,
          "_kuzzle_info": {
            "author": "-1",
            "createdAt": 1545411356404,
            "updatedAt": null,
            "updater": null,
            "active": true,
            "deletedAt": null
          }
        }
      },
      {
        "_index": "nyc-open-data",
        "_type": "yellow-taxi",
        "_id": "some-other-id",
        "_version": 1,
        "found": true,
        "_source": {
          "capacity": 7,
          "_kuzzle_info": {
            "author": "-1",
            "createdAt": 1545411356424,
            "updatedAt": null,
            "updater": null,
            "active": true,
            "deletedAt": null
          }
        }
      }
    ]
  */
  Console.WriteLine($"Successfully retrieved {response.Count} documents");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

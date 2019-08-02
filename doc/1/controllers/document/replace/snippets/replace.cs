try {
  await kuzzle.Document.create(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{""color"": ""yellow""}"));

  string response = await kuzzle.Document.ReplaceAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{
      ""capacity"": 4,
      ""category"": ""sedan""
    }"));

  Console.WriteLine(response.ToString());
  /*
  {
    "_index": "nyc-open-data",
    "_type": "yellow-taxi",
    "_id": "some-id",
    "_version": 2,
    "result": "updated",
    "_source": {
      "capacity": 4,
      "category": "sedan",
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1538641029988,
        "updatedAt": 1538641029988,
        "updater": "-1",
        "active": true,
        "deletedAt": null
      }
    }
  }
  */
  Console.WriteLine("Document successfully replaced");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e.Message);
}

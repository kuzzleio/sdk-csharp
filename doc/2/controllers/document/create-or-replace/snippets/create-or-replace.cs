try {
  JObject result = await kuzzle.Document.CreateOrReplaceAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{
      ""lastName"": ""McHan""
    }"));

  Console.WriteLine(result.ToString());
  /*
  {
    "_index": "nyc-open-data",
    "_type": "yellow-taxi",
    "_id": "some-id",
    "_version": 1,
    "result": "created",
    "created": true,
    "_source": {
      "lastName": "McHan",
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1537445737667,
        "updatedAt": null,
        "updater": null,
        "active": true,
        "deletedAt": null
      }
    }
  }
  */

  Console.WriteLine("Document successfully created");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

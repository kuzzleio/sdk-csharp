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
    "_id": "some-id",
    "_version": 1,
    "result": "created",
    "_source": {
      "lastName": "McHan",
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1537445737667,
        "updatedAt": null,
        "updater": null
      }
    }
  }
  */

  Console.WriteLine("Document successfully created");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

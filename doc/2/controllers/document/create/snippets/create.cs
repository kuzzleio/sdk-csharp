try {
  JObject result = await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{
      ""lastname"": ""Eggins""
    }"),
    id: "some-id");

  Console.WriteLine(result.ToString());
  /*
  {
    "_id": "some-id",
    "_version": 1,
    "_source": {
      "lastName": "Eggins",
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

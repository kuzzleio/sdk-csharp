try {
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{""color"": ""yellow""}"),
    id: "some-id");

  JObject response = await kuzzle.Document.ReplaceAsync(
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
    "_id": "some-id",
    "_version": 2,
    "_source": {
      "capacity": 4,
      "category": "sedan",
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1538641029988,
        "updatedAt": 1538641029988,
        "updater": "-1"
      }
    }
  }
  */
  Console.WriteLine("Document successfully replaced");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

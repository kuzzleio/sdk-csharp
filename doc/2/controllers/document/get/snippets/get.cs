try {
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{""capacity"": 4}"),
    id: "some-id");

  JObject response =
    await kuzzle.Document.GetAsync("nyc-open-data", "yellow-taxi", "some-id");

  Console.WriteLine(response.ToString());
  /*
  {
    "_id":"some-id",
    "_version":1,
    "_source":{
        "capacity":4,
        "_kuzzle_info":{
          "author":"-1",
          "createdAt":1538402859880,
          "updatedAt":null,
          "updater":null
        }
    }
  }
  */

  Console.WriteLine("Success");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

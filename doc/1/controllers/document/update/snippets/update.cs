try {
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{""capacity"": 4}"));

  JObject response = await kuzzle.Document.UpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{""category"": ""suv""}"));

  Console.WriteLine(response.ToString());
  /*
  {
    "_index": "nyc-open-data",
    "_type": "yellow-taxi",
    "_id": "some-id",
    "_version": 2,
    "result": "updated"
  }
  */
  Console.WriteLine("Document successfully updated");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e.Message);
}

try {
  await kuzzle.Document.CreateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{""capacity"": 4}"),
    id: "some-id");

  JObject response = await kuzzle.Document.UpdateAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id",
    JObject.Parse(@"{""category"": ""suv""}"));

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "_id": "some-id",
    "_version": 2
  }
  */
  Console.WriteLine("Document successfully updated");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

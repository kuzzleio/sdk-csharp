try {
  bool valid = await kuzzle.Document.ValidateAsync(
    "nyc-open-data",
    "yellow-taxi",
    JObject.Parse(@"{
      ""capacity"": 4
    }"));

  if (valid) {
    Console.WriteLine("The document is valid");
  }
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

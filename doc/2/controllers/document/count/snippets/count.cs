try {
  JObject query = JObject.Parse(@"{
      ""query"": {
        ""match"": {
          ""license"": ""valid""
        }
      }
    }");

  int count = await kuzzle.Document.CountAsync("nyc-open-data", "yellow-taxi", query);

  Console.WriteLine($"Found {count} documents matching license:valid");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}

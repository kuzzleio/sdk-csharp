try {
  JObject mapping = JObject.Parse(@"
  {
    properties: {
      license: { type: 'keyword' },
      driver: {
        properties: {
          name: { type: 'keyword' },
          curriculum: { type: 'text' }
        }
      }
    }
  }");

  await kuzzle.Collection.CreateAsync("nyc-open-data", "yellow-taxi", mapping);

  Console.WriteLine("Collection successfully created");
} catch (Exception e) {
  Console.WriteLine(e);
}

try {
  JObject mappings = JObject.Parse(@"{
    properties: {
      plate: { 
        type: 'keyword'
      }
    }
  }");

  await kuzzle.Collection.UpdateMappingAsync("nyc-open-data", "yellow-taxi", mappings);

  Console.WriteLine("Mapping successfully updated");
} catch (Exception e) {
  Console.WriteLine(e);
}

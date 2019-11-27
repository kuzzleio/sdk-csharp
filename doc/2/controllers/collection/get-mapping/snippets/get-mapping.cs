try {
  JObject mapping = await kuzzle.Collection.GetMappingAsync("nyc-open-data", "yellow-taxi");

  Console.WriteLine(mapping["properties"]?.ToString(Formatting.None));
  // {"properties":{"license":{"type":"keyword"},"driver":{"properties":{"name":{"type":"keyword"},"curriculum":{"type":"text"}}}}}
} catch (Exception e) {
  Console.WriteLine(e);
}

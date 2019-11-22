try {
  JObject specifications = await kuzzle.Collection.GetSpecificationsAsync("nyc-open-data", "yellow-taxi");

  Console.WriteLine(specifications.ToString(Formatting.None));
  // {"nyc-open-data": {"yellow-taxi": {"strict": false, "fields": {"license": {"type": "string"}}}}}
} catch (Exception e) {
  Console.WriteLine(e);
}

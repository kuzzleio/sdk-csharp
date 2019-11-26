try {
  JObject specifications = JObject.Parse(@"{
    strict: false,
    fields: {
      license: {
        mandatory: true,
        type: 'string'
      }
    }
  }");

  bool validation_response =
    await kuzzle.Collection.ValidateSpecificationsAsync(
      "nyc-open-data",
      "yellow-taxi",
      specifications);

  Console.WriteLine("Success");
} catch (Exception e) {
  Console.WriteLine(e);
}

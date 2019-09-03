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
      specifications
    );

  Console.WriteLine("Success");
} catch (Exception e) {
  Console.WriteLine(e);
}

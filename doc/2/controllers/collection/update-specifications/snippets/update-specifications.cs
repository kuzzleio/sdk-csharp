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

  JObject updatedSpecifications =
    await kuzzle.Collection.UpdateSpecificationsAsync(
      "nyc-open-data",
      "yellow-taxi",
      specifications
    );

  Console.WriteLine(updatedSpecifications.ToString(Formatting.None));
  /*
  {
    "strict": false,
    "fields": {
      "license": {
        "mandatory": true,
        "type": "string"
      }
    }
  }
  */
} catch (Exception e) {
  Console.WriteLine(e);
}

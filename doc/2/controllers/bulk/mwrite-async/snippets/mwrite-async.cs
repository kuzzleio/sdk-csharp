try {
    JObject response = await kuzzle.Bulk.MWriteAsync(
        "nyc-open-data",
        "yellow-taxi",
        JArray.Parse("[{_id: 'foo', body: {}}]"));

    Console.WriteLine(response.ToString(Formatting.None));
    /*
    {
      errors: [],
      successes: [
        {
          _id: "foo",
          _version: 1,
          _source: {},
          created: true
        }
      ]
    }
    */
} catch (KuzzleException e) {
    Console.WriteLine(e);
}
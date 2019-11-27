try {
    JArray bulkData = JArray.Parse(@"
    [
      { index: { } },
      { a: 'document', with: 'any', number: 'of fields' },

      { create: { _id: 'uniq-id-1' } },
      { another: 'document' },

      { create: { _id: 'uniq-id-2' } },
      { and: { another: 'one' } }
    ]");
    JObject response = await kuzzle.Bulk.ImportAsync(
      "nyc-open-data",
      "yellow-taxi",
      bulkData);
    Console.WriteLine(response.ToString(Formatting.None));

    /*
    {
      errors: [],
      successes: [
        {
          index: {
            _id: "hQ10_GwBB2Y5786Pu_NO",
            status: 201
          }
        },
        {
          create: {
            _id: "uniq-id-1",
            status: 201
          }
        },
        {
          create: {
            _id: "uniq-id-2",
            status: 201
          }
        }
      ]
    }
    */

} catch (KuzzleException e) {
    Console.WriteLine(e);
}
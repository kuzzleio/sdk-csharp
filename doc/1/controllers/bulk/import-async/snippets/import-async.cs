try {
    JArray bulkData = JArray.Parse(@"
    [{ create: { _id: '1', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { a: 'document', with: 'any', number: 'of fields' },
    { create: { _id: '2', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { another: 'document' },
    { create: { _id: '3', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { and: { another: 'one' } }]");
    await kuzzle.Bulk.ImportAsync("foo", "bar", bulkData);
    Console.WriteLine("Success");
} catch (Exception e) {
    Console.WriteLine(e);
}
try {
    await kuzzle.Bulk.MWriteAsync("nyc-open-data", "yellow-taxi", JArray.Parse("[{_id: 'foo', body: {}}]"));
    Console.WriteLine("Success");
} catch (Exception e) {
    Console.WriteLine(e);
}
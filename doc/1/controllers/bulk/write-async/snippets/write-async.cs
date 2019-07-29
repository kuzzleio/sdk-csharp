try {
    await kuzzle.Bulk.WriteAsync("nyc-open-data", "yellow-taxi", JObject.Parse("{foo: 'bar'}"));
    Console.WriteLine("Success");
} catch (Exception e) {
    Console.WriteLine(e);
}
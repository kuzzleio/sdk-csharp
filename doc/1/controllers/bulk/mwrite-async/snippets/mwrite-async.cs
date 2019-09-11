try {
    JObject response = await kuzzle.Bulk.MWriteAsync(
        "nyc-open-data",
        "yellow-taxi",
        JArray.Parse("[{_id: 'foo', body: {}}]"));
    Console.WriteLine(response.ToString(Formatting.None));
    /*
    {
        "hits": [
            {
            "_id": "foo",
            "_source": {},
            "_index": "nyc-open-data",
            "_type": "yellow-taxi",
            "_version": 1,
            "result": "created",
            "_shards": {
                "total": 2,
                "successful": 1,
                "failed": 0
            },
            "created": true,
            "status": 201
            }
        ],
        "total": 1
    }
    */
} catch (KuzzleException e) {
    Console.WriteLine(e);
}
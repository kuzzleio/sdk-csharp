try {
    JArray bulkData = JArray.Parse(@"
    [{ create: { _id: '1', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { a: 'document', with: 'any', number: 'of fields' },
    { create: { _id: '2', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { another: 'document' },
    { create: { _id: '3', _index: 'nyc-open-data', _type: 'yellow-taxi' } },
    { and: { another: 'one' } }]");
    JObject response = await kuzzle.Bulk.ImportAsync("foo", "bar", bulkData);
    Console.WriteLine(response.ToString(Formatting.None));
    
    /*
    {
      "took": 49,
      "errors": false,
      "items": [
        {
          "create": {
            "_index": "nyc-open-data",
            "_type": "yellow-taxi",
            "_id": "1",
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
        },
        {
          "create": {
            "_index": "nyc-open-data",
            "_type": "yellow-taxi",
            "_id": "2",
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
        },
        {
          "create": {
            "_index": "nyc-open-data",
            "_type": "yellow-taxi",
            "_id": "3",
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
        }
      ]
    }
    */
    
} catch (Exception e) {
    Console.WriteLine(e);
}
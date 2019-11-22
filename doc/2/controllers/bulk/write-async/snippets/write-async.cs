try {
    JObject content = JObject.Parse(@"{
      _kuzzle_info: {
        author: '<kuid>',
        createdAt: 1481816934209,
        updatedAt: null,
        updater: null,
        active: true,
        deletedAt: null
      }
    }");
    JObject response = await kuzzle.Bulk.WriteAsync(
        "nyc-open-data",
        "yellow-taxi",
        content);
    Console.WriteLine(response.ToString(Formatting.None));
    /*
    {
        "_index": "nyc-open-data",
        "_type": "yellow-taxi",
        "_id": "AWxHzUJ4wXgLgoMjxZ3S",
        "_version": 1,
        "result": "created",
        "_shards": {
            "total": 2,
            "successful": 1,
            "failed": 0
        },
        "created": true,
        "_source": {
            "kuzzle_info": {
            "author": "<kuid>",
            "createdAt": 1481816934209,
            "updatedAt": null,
            "updater": null,
            "active": true,
            "deletedAt": null
            }
        }
    }
    */
} catch (KuzzleException e) {
    Console.WriteLine(e);
}
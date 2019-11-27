try {
    JObject content = JObject.Parse(@"{
      _kuzzle_info: {
        author: '<kuid>',
        createdAt: 1481816934209,
        updatedAt: null,
        updater: null
      }
    }");
    JObject response = await kuzzle.Bulk.WriteAsync(
        "nyc-open-data",
        "yellow-taxi",
        content);
    Console.WriteLine(response.ToString(Formatting.None));
    /*
    {
      "_id": "AWxHzUJ4wXgLgoMjxZ3S",
      "_version": 1,
      "_source": {
        "kuzzle_info": {
          "author": "<kuid>",
          "createdAt": 1481816934209,
          "updatedAt": null,
          "updater": null
        }
      }
    }
    */
} catch (KuzzleException e) {
    Console.WriteLine(e);
}
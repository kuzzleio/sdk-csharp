JObject request = JObject.Parse(@"{
  controller: 'document',
  action: 'create',
  index: 'nyc-open-data',
  collection: 'yellow-taxi',
  _id: 'my-custom-document-id',
  refresh: 'wait_for',
  body: {
    trip_distance: 4.23,
    passenger_count: 2
  }
}");

Response response = await kuzzle.QueryAsync(request);

Console.WriteLine(response.ToString());

/*
  { requestId: '49ffb6db-bdff-45b9-b3f6-00442f472393',
    status: 200,
    error: null,
    controller: 'document',
    action: 'create',
    collection: 'yellow-taxi',
    index: 'nyc-open-data',
    volatile: { sdkName: 'csharp@2.0.0' },
    room: '49ffb6db-bdff-45b9-b3f6-00442f472393',
    result:
      { _id: 'my-custom-document-id',
        _version: 1,
        result: 'created',
        _source:
        { trip_distance: 4.23,
          passenger_count: 2,
          _kuzzle_info:
            { author: '-1',
              createdAt: 1532529302225,
              updatedAt: null,
              updater: null } } } }
*/

Console.WriteLine("Document created");

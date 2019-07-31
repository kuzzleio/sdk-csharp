try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject user = await kuzzle.Auth.GetCurrentUserAsync();

  Console.WriteLine(user.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "_source": {
      "profileIds": [
        "default"
      ],
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1564563030235,
        "updatedAt": null,
        "updater": null
      }
    },
    "_meta": {
      "author": "-1",
      "createdAt": 1564563030235,
      "updatedAt": null,
      "updater": null
    },
    "strategies": [
      "local"
    ]
  }
  */

  Console.WriteLine("Successfully got current user");
} catch (Exception e) {
  Console.WriteLine(e);
}

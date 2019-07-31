try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject updatedUser = await kuzzle.Auth.UpdateSelfAsync(JObject.Parse("{age: 42}"));

  Console.WriteLine(updatedUser.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "_source": {
      "profileIds": [
        "default"
      ],
      "_kuzzle_info": {
        "author": "-1",
        "createdAt": 1564566023173,
        "updatedAt": null,
        "updater": null
      },
      "age": 42
    },
    "_meta": {
      "author": "-1",
      "createdAt": 1564566023173,
      "updatedAt": null,
      "updater": null
    }
  }
  */
} catch (Exception e) {
  Console.WriteLine(e);
}

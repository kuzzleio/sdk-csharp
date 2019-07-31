try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));

  JArray rights = await kuzzle.Auth.GetMyRightsAsync();

  Console.WriteLine(rights.ToString(Formatting.None));
  /*
  [
    {
      "controller": "*",
      "action": "*",
      "collection": "*",
      "index": "*",
      "value": "allowed"
    }
  ]
  */
} catch (Exception e) {
  Console.WriteLine(e);
}

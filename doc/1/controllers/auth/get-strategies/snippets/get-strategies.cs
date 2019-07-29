try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JArray strategies = await kuzzle.Auth.GetStrategiesAsync();

  foreach (string strategy in strategies) {
    Console.WriteLine(strategy);
  }
} catch (Exception e) {
  Console.WriteLine(e);
}

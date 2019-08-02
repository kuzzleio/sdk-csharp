try {
  await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));
  JArray strategies = await kuzzle.Auth.GetStrategiesAsync();

  Console.WriteLine(strategies.ToString(Formatting.None));
  /*
  [
    "local"
  ]
  */
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

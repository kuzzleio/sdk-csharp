try {
  JObject response = await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  JObject requestPayload = new JObject {
    { "controller", "server"},
    { "action", "info" }
  };

  JObject res = await kuzzle.Auth.CheckRightsAsync(requestPayload);

  Console.WriteLine(res.ToString(Formatting.None));
  /*
  {
    "allowed": true,
  }
  */
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

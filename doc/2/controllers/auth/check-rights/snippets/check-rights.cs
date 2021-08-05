try {
  JObject response = await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  JObject requestPayload = new JObject {
    { "controller", "server"},
    { "action", "info" }
  };

  bool allowed = await kuzzle.Auth.CheckRightsAsync(requestPayload);

  Console.WriteLine(allowed);
  /*
    true
  */
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

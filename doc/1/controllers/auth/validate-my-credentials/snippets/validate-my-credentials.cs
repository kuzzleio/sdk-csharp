try {
  await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));
  bool valid = await kuzzle.Auth.ValidateMyCredentialsAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  if (valid) {
    Console.WriteLine("Credentials are valid");
  }
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

try {
  await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject local_credentials = await kuzzle.Auth.GetMyCredentialsAsync("local");

  Console.WriteLine(local_credentials.ToString(Formatting.None));
  /*
  {
    "username": "foo",
    "kuid": "foo"
  }
  */
  Console.WriteLine("Successfully got local credentials");
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

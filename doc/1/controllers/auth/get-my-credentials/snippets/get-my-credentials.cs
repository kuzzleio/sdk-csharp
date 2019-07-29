try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject local_credentials = await kuzzle.Auth.GetMyCredentialsAsync("local");

  Console.WriteLine(local_credentials);
  Console.WriteLine("Successfully got local credentials");
} catch (Exception e) {
  Console.WriteLine(e);
}

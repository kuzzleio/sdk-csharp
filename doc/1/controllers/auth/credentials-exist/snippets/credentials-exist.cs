try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  bool exists = await kuzzle.Auth.CredentialsExistAsync("local");

  if (exists) {
    Console.WriteLine("Credentials exists for local strategy");
  }
} catch (Exception e) {
  Console.WriteLine(e);
}

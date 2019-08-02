try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  await kuzzle.Auth.DeleteMyCredentialsAsync("local");

  Console.WriteLine("Credentials Successfully deleted");
} catch (Exception e) {
  Console.WriteLine(e);
}

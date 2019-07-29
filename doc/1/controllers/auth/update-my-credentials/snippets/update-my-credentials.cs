try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  await kuzzle.Auth.UpdateMyCredentialsAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  Console.WriteLine("Credentials successfully updated");
} catch (Exception e) {
  Console.WriteLine(e);
}

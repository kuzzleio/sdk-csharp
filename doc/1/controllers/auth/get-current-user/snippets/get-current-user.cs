try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject user = await kuzzle.Auth.GetCurrentUserAsync();

  Console.WriteLine("Successfully got current user");
} catch (Exception e) {
  Console.WriteLine(e);
}

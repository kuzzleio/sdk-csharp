try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));

  Console.WriteLine("Success");
} catch (Exception e) {
  Console.WriteLine(e);
}

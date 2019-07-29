try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject updatedUser = await kuzzle.Auth.UpdateSelfAsync(JObject.Parse("{age: 42}"));

  Console.WriteLine("Success");
} catch (Exception e) {
  Console.WriteLine(e);
}

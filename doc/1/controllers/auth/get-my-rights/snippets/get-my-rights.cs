try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));

  JArray rights = await kuzzle.Auth.GetMyRightsAsync();

  foreach (JObject right in rights) {
    Console.WriteLine(right["controller"]?.ToString() + " " + right["action"]?.ToString());
    Console.WriteLine(right["index"]?.ToString() + " " + right["collection"]?.ToString());
    Console.WriteLine(right["value"]?.ToString());
  }
} catch (Exception e) {
  Console.WriteLine(e);
}

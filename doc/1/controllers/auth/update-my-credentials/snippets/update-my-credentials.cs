try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
  JObject response = await kuzzle.Auth.UpdateMyCredentialsAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "username": "foo",
    "kuid": "foo"
  }
  */
  Console.WriteLine("Credentials successfully updated");
} catch (Exception e) {
  Console.WriteLine(e);
}

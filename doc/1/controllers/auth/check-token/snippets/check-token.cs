try {
  JObject response = (await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}")));

  JObject res = await kuzzle.Auth.CheckTokenAsync(response["jwt"]?.ToString());

  Console.WriteLine(res.ToString(Formatting.None));
  /*
  {
    "valid": true,
    "expiresAt": 1564563452570
  }
  */
} catch (Exception e) {
  Console.WriteLine(e);
}

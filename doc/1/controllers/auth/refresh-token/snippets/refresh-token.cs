try {
  await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  JObject response = await kuzzle.Auth.RefreshTokenAsync();

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "jwt": "eyJhbGciOiJI.eyJfaWQiOiJmb28iL.qOTDPUH7So9QL0qaMkUsyTWRhjHd",
    "expiresAt": 1564569366389,
    "ttl": 3600000
  }
  */
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

try {
  JObject response = await kuzzle.Auth.LoginAsync(
    "local",
    JObject.Parse("{username: 'foo', password: 'bar'}"));

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfa...",
    "expiresAt": 1564568979921,
    "ttl": 3600000
  }
  */
  Console.WriteLine("Successfuly logged in");
} catch (KuzzleException e) {
  Console.WriteLine(e);
}

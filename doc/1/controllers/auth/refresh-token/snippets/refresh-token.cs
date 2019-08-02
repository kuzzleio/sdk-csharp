try {
  await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));

  JObject response = await kuzzle.Auth.RefreshTokenAsync();

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiJmb28iLCJpYXQiOjE1NjQ1NjU3NjYsImV4cCI6MTU2NDU2OTM2Nn0.TXi2urRSuy8FWUBxQZAJVRv8yUjyCiUFSrXFOT4FeyQ",
    "expiresAt": 1564569366389,
    "ttl": 3600000
  }
  */
} catch (Exception e) {
  Console.WriteLine(e);
}

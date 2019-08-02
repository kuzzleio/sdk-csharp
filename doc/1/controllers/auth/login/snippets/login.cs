try {
  JObject response = await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));

  Console.WriteLine(response.ToString(Formatting.None));
  /*
  {
    "_id": "foo",
    "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiJmb28iLCJpYXQiOjE1NjQ1NjUzNzksImV4cCI6MTU2NDU2ODk3OX0.qOTDPUH7So9QL0qaMkUsyTWRhjHdfFffSPrk4QWQxGw",
    "expiresAt": 1564568979921,
    "ttl": 3600000
  }
  */
  Console.WriteLine("Successfuly logged in");
} catch (Exception e) {
  Console.WriteLine(e);
}

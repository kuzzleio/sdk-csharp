try {
  JObject response = (await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}")));

  JObject res = await kuzzle.Auth.CheckTokenAsync(response["jwt"].ToString());

  if (res["valid"] != null && (bool)res["valid"])
    Console.WriteLine("Success");
  else
    Console.WriteLine(res["state"].ToString());
} catch (Exception e) {
  Console.WriteLine(e);
}

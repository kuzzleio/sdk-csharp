    await kuzzle.Auth.LoginAsync("local", JObject.Parse("{username: 'foo', password: 'bar'}"));
    await kuzzle.Auth.CreateMyCredentialsAsync("other", JObject.Parse("{username: 'foo', password: 'bar'}"));
    Console.WriteLine("Success");
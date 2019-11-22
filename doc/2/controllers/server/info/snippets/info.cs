JObject info = await kuzzle.Server.InfoAsync();

Console.WriteLine("Kuzzle server informations: " +
    info.ToString(Formatting.None));
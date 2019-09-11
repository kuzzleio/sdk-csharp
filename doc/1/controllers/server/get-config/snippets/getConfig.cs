JObject config = await kuzzle.Server.GetConfigAsync();

Console.WriteLine("Kuzzle Server configuration: " +
    config.ToString(Formatting.None));
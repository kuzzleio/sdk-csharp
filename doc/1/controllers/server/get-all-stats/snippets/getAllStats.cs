JObject allStats = await kuzzle.Server.GetAllStatsAsync();

Console.WriteLine("All Kuzzle Stats: " + allStats.ToString(Formatting.None));


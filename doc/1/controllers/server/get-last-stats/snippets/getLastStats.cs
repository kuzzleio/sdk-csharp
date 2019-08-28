JObject lastStats = await kuzzle.Server.GetLastStatsAsync();

Console.WriteLine("Last Kuzzle stats: " + lastStats.ToString(Formatting.None));
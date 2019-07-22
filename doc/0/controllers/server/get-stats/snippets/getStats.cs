long start = 1234567890000L;
long end = 1541426610000L;
JObject stats = await kuzzle.Server.GetStatsAsync(start, end);

Console.WriteLine("Kuzzle stats: " + stats.ToString(Formatting.None));
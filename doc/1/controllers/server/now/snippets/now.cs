Int64 now = await kuzzle.Server.NowAsync();

Console.WriteLine("Epoch-millis timestamp: " + now);
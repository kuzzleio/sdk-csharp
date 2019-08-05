await kuzzle.Index.RefreshAsync("nyc-open-data");
Console.WriteLine("All shards refreshed the index");

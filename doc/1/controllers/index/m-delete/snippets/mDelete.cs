JArray indexes = JArray.Parse("['nyc-open-data', 'mtp-open-data']");
JArray deleted = await kuzzle.Index.MDeleteAsync(indexes);
Console.WriteLine($"Successfully deleted {deleted.Count} indexes");

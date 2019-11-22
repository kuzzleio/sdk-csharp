JArray indexes = await kuzzle.Index.ListAsync();
Console.WriteLine($"Kuzzle contains {indexes.Count} indexes");

try {
  JObject collection_list = await kuzzle.Collection.ListAsync("mtp-open-data", 1, 2);

  Console.WriteLine(collection_list.ToString(Formatting.None));
  // {"type":"all","collections":[{"name":"pink-taxi","type":"stored"}],"from":1,"size":2}
} catch (Exception e) {
  Console.WriteLine(e);
}

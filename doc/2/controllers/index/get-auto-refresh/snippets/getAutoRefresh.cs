if (await kuzzle.Index.GetAutoRefreshAsync("nyc-open-data")) {
  Console.WriteLine("autorefresh is true");
} else {
  Console.WriteLine("autorefresh is false");
}

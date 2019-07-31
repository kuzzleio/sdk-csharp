try {
  JObject message = JObject.Parse("{ realtime: 'rule the web' }");

  await kuzzle.Realtime.PublishAsync("i-dont-exist", "in-database", message);

  Console.WriteLine("Message successfully published");
} catch (Exception e) {
  Console.WriteLine(e);
}

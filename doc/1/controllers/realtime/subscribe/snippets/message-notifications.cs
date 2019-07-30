NotificationHandler listener = (notification) => {
  Console.WriteLine("Message notification received");
  Console.WriteLine(notification.Result);
  /*
  {
  "_source": {
    "metAt": "Insane",
    "hello": "world",
    "_kuzzle_info": {
      "author": "-1",
      "createdAt": 1564474215073
    }
  },
  "_id": null
  } */
};

try {
  // Subscribes to notifications for documents
  await kuzzle.Realtime.SubscribeAsync("i-dont-exist", "in-database", JObject.Parse("{}"), listener);

  JObject message = JObject.Parse("{ metAt: 'Insane', hello: 'world' }");
  await kuzzle.Realtime.PublishAsync("i-dont-exist", "in-database", message);
} catch (Exception e) {
  Console.WriteLine(e);
}

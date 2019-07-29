kuzzle.Offline.MaxQueueSize = 10;
kuzzle.Offline.MinTokenDuration = 36000000;
kuzzle.Offline.MaxRequestDelay = 5000;
kuzzle.Offline.QueueFilter = (obj) => obj["action"]?.ToString() == "login";

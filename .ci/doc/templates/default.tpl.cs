#r "Kuzzle.dll"
using System;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Protocol;
using static KuzzleSdk.API.Controllers.RealtimeController;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

WebSocket socket = new WebSocket(new Uri("ws://kuzzle:7512"));
KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(socket);

kuzzle.ConnectAsync(CancellationToken.None).Wait();

[snippet-code]

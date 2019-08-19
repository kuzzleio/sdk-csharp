#r "Kuzzle.dll"
using System;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Protocol;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KuzzleSdk.API.Options;
using KuzzleSdk.API.DataObjects;

WebSocket socket = new WebSocket(new Uri("ws://kuzzle:7512"));
KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(socket);

kuzzle.ConnectAsync(CancellationToken.None).Wait();

[snippet-code]

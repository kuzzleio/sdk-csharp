using System;
using System.Threading;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Protocol;

WebSocket networkProtocol = new WebSocket(new Uri("ws://kuzzle:7512"));
KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(networkProtocol);

[![Build Status](https://travis-ci.org/kuzzleio/sdk-csharp.svg?branch=master)](https://travis-ci.org/kuzzleio/sdk-csharp)
[![codecov.io](http://codecov.io/github/kuzzleio/sdk-csharp/coverage.svg?branch=master)](http://codecov.io/github/kuzzleio/sdk-csharp?branch=master)


## About

### Kuzzle C# SDK

This is the official C# SDK for the free and open-source backend Kuzzle. It provides a way to dial with a Kuzzle server from C# applications.

#### Asynchronous

All SDK methods are asynchronous using C# `Task`.  
For example, for the action create of the controller collection (_collection:create_), the API result contains `{ "acknowledged": true }` . This is therefore what will be returned inside a `JObject` by the SDK method if successful.  

<p align="center">
  :books: <b><a href="https://docs.kuzzle.io/sdk-reference/csharp/1/">Documentation</a></b>
</p>

### Kuzzle

Kuzzle is an open-source backend that includes a scalable server, a multiprotocol API,
an administration console and a set of plugins that provide advanced functionalities like real-time pub/sub, blazing fast search and geofencing.

* :octocat: __[Github](https://github.com/kuzzleio/kuzzle)__
* :earth_africa: __[Website](https://kuzzle.io)__
* :books: __[Documentation](https://docs-v2.kuzzle.io)__
* :email: __[Discord](http://join.discord.kuzzle.io)__


#### Get trained by the creators of Kuzzle :zap:

Train yourself and your teams to use Kuzzle to maximize its potential and accelerate the development of your projects.  
Our teams will be able to meet your needs in terms of expertise and multi-technology support for IoT, mobile/web, backend/frontend, devops.  
:point_right: [Get a quote](https://hubs.ly/H0jkfJ_0)

## Usage

### Compatibility matrix

| Kuzzle Version | SDK Version |
|----------------|-------------|
| 1.x.x          | 1.x.x       |
| 2.x.x          | 2.x.x       |

### Getting started :point_right:

  - [Standalone](https://docs.kuzzle.io/sdk/csharp/2/getting-started/standalone/)

### Installation

#### NuGet

The SDK is available on [NuGet](https://www.nuget.org/packages/kuzzlesdk/).  

### Example

```csharp
using KuzzleSdk;
using KuzzleSdk.Protocol;

WebSocket socket = new WebSocket(new Uri("ws://kuzzle:7512"));
KuzzleSdk.Kuzzle kuzzle = new KuzzleSdk.Kuzzle(socket);

await kuzzle.ConnectAsync(CancellationToken.None);

Int64 now = await kuzzle.Server.NowAsync();
Console.WriteLine("Epoch-millis timestamp: " + now);
```

## Compile & test

Preprequisites:
- .NET Core SDK 2.1 

Compile with the following command lines:

```
$ dotnet restore
$ dotnet build Kuzzle/Kuzzle.csproj -c Release
```

To start the unit tests using the command line:

```
$ dotnet test
```

### monodevelop

If you're using monodevelop, you'll need at least mono 5.20+ (w/ msbuild 16+). Due to compatibility problems, you HAVE TO install .NET Core SDK 2.1, if you only have the 2.2 one, you won't be able to build the project with msbuild (which monodevelop uses).

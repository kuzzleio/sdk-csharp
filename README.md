Official Kuzzle C# SDK - ALPHA VERSION

======

## About Kuzzle

Kuzzle is a ready-to-use, **on-premises backend** that enables you to manage your persistent data and be notified in real-time on whatever happens to it. It also provides you with a flexible and powerful user-management system.

Kuzzle enables you to build modern web applications and complex IoT networks in no time.

Official website: https://kuzzle.io
Documentation: https://docs.kuzzle.io

## SDK Documentation

The complete SDK documentation is available [here](http://docs.kuzzle.io/sdk-reference/)

## Protocol used

The C# SDK implements the websocket protocol.

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

## monodevelop

If you're using monodevelop, you'll need at least mono 5.20+ (w/ msbuild 16+). Due to compatibility problems, you HAVE TO install .NET Core SDK 2.1, if you only have the 2.2 one, you won't be able to build the project with msbuild (which monodevelop uses).


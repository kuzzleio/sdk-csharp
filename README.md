[![Build Status](https://travis-ci.org/kuzzleio/sdk-csharp.svg?branch=master)](https://travis-ci.org/kuzzleio/sdk-csharp)
[![codecov.io](http://codecov.io/github/kuzzleio/sdk-csharp/coverage.svg?branch=master)](http://codecov.io/github/kuzzleio/sdk-csharp?branch=master)
[![Join the chat at https://gitter.im/kuzzleio/kuzzle](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/kuzzleio/kuzzle?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)


Official Kuzzle C# SDK - ALPHA VERSION

## About Kuzzle

Kuzzle is a ready-to-use, **on-premises backend** that enables you to manage your persistent data and be notified in real-time on whatever happens to it. It also provides you with a flexible and powerful user-management system.

Kuzzle enables you to build modern web applications and complex IoT networks in no time.

* :watch: __[Kuzzle in 5 minutes](https://kuzzle.io/company/about-us/kuzzle-in-5-minutes/)__
* :octocat: __[Github](https://github.com/kuzzleio/kuzzle)__
* :earth_africa: __[Website](https://kuzzle.io)__
* :books: __[Documentation](https://docs.kuzzle.io)__
* :email: __[Gitter](https://gitter.im/kuzzleio/kuzzle)__

## Get trained by the creators of Kuzzle :zap:

Train yourself and your teams to use Kuzzle to maximize its potential and accelerate the development of your projects.  
Our teams will be able to meet your needs in terms of expertise and multi-technology support for IoT, mobile/web, backend/frontend, devops.  
:point_right: [Get a quote](https://hubs.ly/H0jkfJ_0)

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

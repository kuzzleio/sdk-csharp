---
code: false
type: page
title: C# Standalone
description: Getting started with Kuzzle and C#
order: 0
---

# Getting Started with Kuzzle and C#

This tutorial explains how to use **Kuzzle** with **C#**, **.NET Core SDK 2.1** and the **Kuzzle C# SDK**.
It will walk you through creating scripts that can **store** documents in Kuzzle and subscribe to **notifications** about document creations.

You are going to write an application that **stores** documents in Kuzzle Server and subscribes to **real time notifications** for each created document.

To follow this tutorial, you must have a Kuzzle Server up and running. Follow these instructions if this is not already the case: [Running Kuzzle](/core/2/guides/getting-started/running-kuzzle/).


::: info
Having trouble? Get in touch with us on [Discord](http://join.discord.kuzzle.io)!
:::

## Explore the SDK

It's time to get started with the [Kuzzle C# SDK](/sdk/csharp/2). This section explains how to store a document and how to subscribe to notifications in Kuzzle using the C# SDK.

Before proceeding, please make sure your system has **.NET Core SDK** version 2.1 or higher.

::: warning
If you're using monodevelop on Linux, you'll need at least mono 5.20+ (w/ msbuild 16+). Due to compatibility issues, you HAVE TO install .NET Core SDK 2.1, if you only have the 2.2 one, you won't be able to build the project with msbuild (which monodevelop uses).
:::

## Prepare your environment

Create your playground directory, initialize a new console application using dotnet CLI and add a reference to the [Kuzzle SDK package](https://www.nuget.org/packages/kuzzlesdk/).  

```sh
mkdir "kuzzle-playground"
cd "kuzzle-playground"
dotnet new console
dotnet add package kuzzlesdk
```

Then inside the `Program.cs` file, we will create a console application that takes an argument to either initialize the application, subscribe to notification or create a document.

<<< ./snippets/Switch.cs

Then create a `GetSdk` function to instantiate the SDK and connect it to a Kuzzle instance using the WebSocket protocol.

Once done, connect the client to your Kuzzle server using the [Kuzzle.ConnectAsync](/sdk/csharp/2/core-classes/kuzzle/connect-async) method.

<<< ./snippets/Program1.cs:1

:::info
Replace 'kuzzle' which is the Kuzzle server hostname with 'localhost' or with the host name where your Kuzzle server is running.
:::

Now, you need to create a new 'nyc-open-data'  index, holding a new 'yellow-taxi' collection. This structure will be used later on to store data.

<<< ./snippets/Program1.cs:2

Your `Program.cs` file should now look like this:

<<< ./snippets/Program1.cs

This code does the following:

- creates an instance of the SDK
- connects it to Kuzzle running on `kuzzle` (change the hostname if needed) using WebSocket
- creates the `nyc-open-data` index
- creates the `yellow-taxi` collection within the `nyc-open-data` index

Run the code with dotnet:

```bash
dotnet run -- init
```

The console should output the following message:

```bash
nyc-open-data/yellow-taxi ready!
```

:::success
Congratulations! You are now ready to say Hello to the World!
:::

## Create your first "Hello World" document

Complete the `Create` method with the following code:

<<< ./snippets/Program.cs:3

This code does the following:

- creates a new document in the `yellow-taxi` collection, within the `nyc-open-data` index
- logs a success message to the console if everything went fine
- logs an error message if any of the previous actions fails

Run the code with dotnet:

```bash
dotnet run -- create
```

:::success
You have now successfully stored your first document into Kuzzle. You can now open an [Admin Console](http://console.kuzzle.io) to browse your collection and confirm that your document was saved.
:::


## Subscribe to realtime document notifications (pub/sub)

Kuzzle provides pub/sub features that can be used to trigger real-time notifications based on the state of your data (for a deep-dive on notifications check out the [realtime notifications](/sdk/csharp/2/essentials/realtime-notifications/) documentation).

Let's get started. Complete the `Subscribe` method with the following code:

<<< ./snippets/Program.cs:4

Run the code with dotnet:

```bash
dotnet run -- subscribe
```

The program is now running endlessly, waiting for notifications about documents matching its filters, specifically documents that have a `license` field equal to `'B'`.

We added a `await Task.Delay(10000, token.Token);` line after a successfull subscribe to keep the program running until after a notification has been received.

Now in another terminal, launch the program to create a document:

```bash
dotnet run -- create
```

This creates a new document in Kuzzle which, in turn, triggers a [document notification](/core/2/api/essentials/notifications/#documents-changes-messages). That notification is sent by Kuzzle to our other running program, which subscribed to changes occuring in that index and collection.
Check the subscribe program terminal: a new message is printed everytime a document is created.

```bash
New driver Liia with id AWccRe3-DfukVhSzMdUo has B license.
```

:::success
Congratulations! You have just set up your first pub/sub communication!
:::

## Where do we go from here?

Now that you're more familiar with Kuzzle, dive even deeper to learn how to leverage its full capabilities:

- discover what this SDK has to offer by browsing other sections of this documentation
- learn how to use [Koncorde](/core/2/api/koncorde-filters-syntax) to create incredibly fine-grained and blazing-fast subscriptions
- learn how to perform a [basic authentication](/sdk/csharp/2/controllers/auth/login)
- follow our guide to learn how to [manage users, and how to set up fine-grained access control](/core/2/guides/essentials/security/)

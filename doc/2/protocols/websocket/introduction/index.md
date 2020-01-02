---
code: false
type: page
title: WebSocket
description: WebSocket protocol implementation description and properties
order: 0
---

# WebSocket

The WebSocket protocol can be used by an instance of the SDK to communicate with your Kuzzle server.

This protocol allows you to use all the features of Kuzzle, including [real-time notifications](/sdk/csharp/2/essentials/realtime-notifications).

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk.Protocol;
```

## Properties

| Property | Type<br/>(default) | Description | writable |
|--- |--- |--- | --- |
|
| `AutoReconnect` | <pre>bool</pre><br/>(`false`) | Try to reestablish connection on an unexpected network loss | yes |
| `KeepAlive` | <pre>string</pre><br/>(`false`) | Actively keep the connection alive | yes |
| `ReconnectionDelay` | <pre>int</pre><br/>(`1000`) | Number of milliseconds between 2 automatic reconnections attempts | yes |
| `ReconnectionRetries` | <pre>int</pre><br/>(`20`) | Maximum number of automatic reconnections attempts | yes |
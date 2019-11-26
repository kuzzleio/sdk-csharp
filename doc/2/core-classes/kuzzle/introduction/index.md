---
code: false
type: page
title: Kuzzle
description: Kuzzle class
order: 0
---

# Kuzzle

The Kuzzle class is the main class of the SDK.
Once instantiated, it represents a connection to your Kuzzle server.

It gives access to the different features of the SDKs:

- access to the available controllers
- [SDK events](/sdk/csharp/2/essentials/events) handling
- resilience to connection loss
- network request queue management

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk;
```

## Properties

| Property | Type | Description | writable |
|--- |--- |--- | --- |
| `AuthenticationToken` | <pre>string</pre> | Search | Authentication token used for API requests |
| `InstanceId` | <pre>string</pre> | SDK instance unique identifier (send in the `Volatile` data) | no |
| `SdkName` | <pre>string</pre> | SDK name with version number | no |
| `MaxQueueSize` | <pre>int</pre> | Maximum amount of elements that the queue can contains (If set to -1, the size is unlimited) | yes |
| `RefreshedTokenDuration` | <pre>int</pre> | Minimum duration of a Token after refresh (If set to -1 the SDK does not refresh the token automaticaly) | yes |
| `MinTokenDuration` | <pre>int</pre> | Minimum duration of a Token before being automaticaly refreshed (If set to -1 the SDK does not refresh the token automaticaly) | yes |
| `MaxRequestDelay` | <pre>int</pre> | Maximum delay between two requests to be replayed | yes |
| `QueueFilter` | <pre>Func<JObject, bool></pre> | Function to filter the request queue before replaying requests. | yes |
| `AutoRecover` | <pre>bool</pre> | Queue requests when network is down and automatically replay them when the SDK successfully reconnects | yes |


## Network protocol

Each instance of the class communicates with the Kuzzle server through a class representing a network protocol implementation.

The following protocols are available in the SDK:

- [WebSocket](/sdk/csharp/2/protocols/websocket)

## Volatile data

You can tell the Kuzzle SDK to attach a set of "volatile" data to each request. You can set it as an object contained in the `Volatile` property of the Kuzzle object. The response to a request containing volatile data will contain the same data in its `Volatile` property. This can be useful, for example, in real-time notifications for [user join/leave notifications](/core/2/api/essentials/volatile-data) to provide additional informations about the client who sent the request.

Note that you can also set volatile data on a per-request basis (on requests that accept a `Volatile` field in their `options` argument). In this case, per-request volatile data will be merged with the global `Volatile` object set in the constructor. Per-request fields will override global ones.
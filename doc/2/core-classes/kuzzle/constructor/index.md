---
code: false
type: page
title: Kuzzle Constructor 
description: Creates a new Kuzzle object connected to the backend
order: 100
---

# Constructor

Use this constructor to create a new instance of the SDK.
Each instance represent a different connection to a Kuzzle server with specific options.

## Arguments

```csharp
public Kuzzle(
  AbstractProtocol networkProtocol,
  int refreshedTokenDuration = 3600000,
  int minTokenDuration = 3600000,
  int maxQueueSize = -1,
  int maxRequestDelay = 1000,
  bool autoRecover = false,
  Func<JObject, bool> queueFilter = null
)
```

<br/>

| Argument   | Type                | Description                       |
| ---------- | ------------------- | --------------------------------- |
| `networkProtocol` | <pre>AbstractProtocol</pre> | Protocol used by the SDK instance |
| `refreshedTokenDuration` | <pre>int</pre> | Minimum duration of a Token after refresh (If set to -1 the SDK does not refresh the token automaticaly) | yes |
| `minTokenDuration` | <pre>int</pre> | Minimum duration of a Token before being automaticaly refreshed (If set to -1 the SDK does not refresh the token automaticaly) | yes |
| `maxQueueSize` | <pre>int</pre> | Maximum amount of elements that the queue can contains (If set to -1, the size is unlimited) | yes |
| `maxRequestDelay` | <pre>int</pre> | Maximum delay between two requests to be replayed | yes |
| `queueFilter` | <pre>Func<JObject, bool></pre> | Function to filter the request queue before replaying requests | yes |
| `autoRecover` | <pre>bool</pre> | Queue requests when network is down and automatically replay them when the SDK successfully reconnects | yes |

## networkProtocol

The protocol used to connect to the Kuzzle instance.
It can be one of the following available protocols:

- [WebSocket](/sdk/csharp/2/protocols/websocket)

## Return

The `Kuzzle` SDK instance.

## Usage

<<< ./snippets/constructor.cs

---
code: true
type: page
title: constructor
description: Creates a new WebSocket protocol
order: 50
---

# Constructor

Initializes a new instance of the WebSocket class pointing to the Kuzzle server specified by the uri.

## Arguments

```csharp
public WebSocket(Uri uri)
```

<br/>

| Argument  | Type              | Description                  |
| --------- | ----------------- | ---------------------------- |
| `uri`    | <pre>Uri</pre> | URI pointing to a Kuzzle server |

### uri

A Uri object pointing to a Kuzzle server.  

Use `wss://<host>:<ip>` to initiate a secure SSL connection.

## Return

A `WebSocket` protocol instance.

## Usage

<<< ./snippets/constructor.cs

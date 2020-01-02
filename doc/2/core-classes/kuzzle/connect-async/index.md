---
code: true
type: page
title: ConnectAsync
description: Connects the SDK to Kuzzle
---

# ConnectAsync

Connects to Kuzzle using the subsequent protocol `ConnectAsync` method. 

Subsequent call have no effect if the SDK is already connected.

## Arguments

```csharp
public async Task ConnectAsync(CancellationToken cancellationToken);
```

## Returns

Returns a `Task` resolving when the SDK is connected.

## Usage

<<< ./snippets/connect.cs

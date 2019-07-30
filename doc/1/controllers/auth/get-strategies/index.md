---
code: true
type: page
title: GetStrategiesAsync
description: Get all authentication strategies registered in Kuzzle.
---

# GetStrategiesAsync

Get all authentication strategies registered in Kuzzle.

## Signature

```csharp
public async Task<JArray> GetStrategiesAsync();
```

## Return

A JArray representing the available authentication strategies.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/get-strategies.cs

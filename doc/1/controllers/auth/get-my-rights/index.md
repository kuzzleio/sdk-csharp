---
code: true
type: page
title: GetMyRightsAsync
description: Returns the rights for the user linked to the `JSON Web Token`.
---

# GetMyRightsAsync

Returns the rights for the currently logged in user within the SDK instance.

## Arguments

```csharp
public async Task<JArray> GetMyRightsAsync();
```

## Return

A JArray object.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/get-my-rights.cs

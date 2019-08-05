---
code: true
type: page
title: CredentialsExistAsync
description: Check that the current user has credentials for the specified strategy.
---

# CredentialsExistAsync

Check that the current user has credentials for the specified strategy.

## Arguments

```csharp
public async Task<bool> CredentialsExistAsync(string strategy);
```

| Argument   | Type              | Description     |
|------------|-------------------|-----------------|
| `strategy` | <pre>string</pre> | Strategy to use |

## Return

A boolean indicating if credentials exists for the strategy.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/credentials-exist.cs

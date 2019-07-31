---
code: true
type: page
title: CreateMyCredentialsAsync
description: Create the current user's credentials for the specified strategy.
---

# CreateMyCredentialsAsync

Create the current user's credentials for the specified strategy.

## Signature

```csharp
public async Task<JObject> CreateMyCredentialsAsync(
      string strategy,
      JObject credentials);
```

## Arguments

| Arguments     | Type               | Description                          |
|---------------|--------------------|--------------------------------------|
| `strategy`    | <pre>string</pre>  | Strategy to use                      |
| `credentials` | <pre>JObject</pre> | JObject representing the credentials |

## Return

A JObject representing the new credentials.

## Usage

<<< ./snippets/create-my-credentials.cs

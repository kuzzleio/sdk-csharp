---
code: true
type: page
title: UpdateMyCredentialsAsync
description: Update the current user's credentials for the specified strategy.
---

# UpdateMyCredentialsAsync

Update the current user's credentials for the specified strategy. The credentials to send will depend on the authentication plugin and the authentication strategy.

## Signature

```csharp
public async Task<JObject> UpdateMyCredentialsAsync(
      string strategy,
      JObject credentials);
```

## Arguments

| Arguments     | Type               | Description                          |
|---------------|--------------------|--------------------------------------|
| `strategy`    | <pre>string</pre>  | Strategy to use                      |
| `credentials` | <pre>JObject</pre> | JObject representing the credentials |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Return

A JObject representing the updated credentials with the following properties:

| Property   | Type              | Description       |
|------------|-------------------|-------------------|
| `username` | <pre>string</pre> | The Username      |
| `kuid`     | <pre>string</pre> | The user's `kuid` |

## Usage

<<< ./snippets/update-my-credentials.cs

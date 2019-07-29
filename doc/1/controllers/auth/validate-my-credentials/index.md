---
code: true
type: page
title: validateMyCredentials
description: Validate the current user's credentials for the specified strategy.
---

# validateMyCredentials

Validate the current user's credentials for the specified strategy. The `result` field is `true` if the provided credentials are valid; otherwise an error is triggered. This route does not actually create or modify the user credentials. The credentials to send will depend on the authentication plugin and authentication strategy.

## Signature

```csharp
public async Task<bool> ValidateMyCredentialsAsync(
      string strategy,
      JObject credentials
    );
```

## Arguments

| Arguments     | Type               | Description                          |
|---------------|--------------------|--------------------------------------|
| `strategy`    | <pre>string</pre>  | Strategy to use                      |
| `credentials` | <pre>JObject</pre> | JObject representing the credentials |

## Return

A boolean indicating if the credentials are valid.

## Usage

<<< ./snippets/validate-my-credentials.cs

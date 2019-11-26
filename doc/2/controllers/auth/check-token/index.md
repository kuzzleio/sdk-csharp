---
code: true
type: page
title: CheckTokenAsync
description: Checks an authentication Token's validity.
---

# CheckTokenAsync

Checks an  authentication token's validity.

## Arguments

```csharp
public async Task<JObject> CheckTokenAsync(string token);
```

| Argument | Type              | Description |
|----------|-------------------|-------------|
| `token`  | <pre>string</pre> | authentication token   |

## Return

A JObject which has the following properties:

| Property     | Type              | Description                      |
|--------------|-------------------|----------------------------------|
| `valid`      | <pre>bool</pre>   | Token validity                   |
| `state`      | <pre>string</pre> | Explain why the token is invalid |
| `expires_at` | <pre>Int64</pre>  | Token expiration timestamp       |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).


## Usage

<<< ./snippets/check-token.cs

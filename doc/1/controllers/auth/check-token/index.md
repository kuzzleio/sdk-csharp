---
code: true
type: page
title: CheckTokenAsync
description: Checks a JWT Token's validity.
---

# CheckTokenAsync

Checks a JWT Token's validity.

## Signature

```csharp
public async Task<JObject> CheckTokenAsync(string token);
```

## Arguments

| Arguments | Type              | Description |
| --------- | ----------------- | ----------- |
| `token`   | <pre>string</pre> | JWT token   |

## Return

A JObject which has the following properties:

| Property   | Type              | Description                      |
|------------|-------------------|----------------------------------|
| valid      | <pre>bool</pre>   | Token validity                   |
| state      | <pre>string</pre> | Explain why the token is invalid |
| expires_at | <pre>Int64</pre>  | Token expiration timestamp       |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).


## Usage

<<< ./snippets/check-token.cs
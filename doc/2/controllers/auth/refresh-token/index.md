---
code: true
type: page
title: RefreshTokenAsync
description: Refreshes an authentication token.
---

# RefreshTokenAsync

Refreshes an authentication token.

- a valid, non-expired authentication must be provided
- the provided authentication token is revoked
- a new authentication token is generated and returned

## Arguments

```csharp
public async Task<JObject> RefreshTokenAsync(TimeSpan? expiresIn = null);
```

**Optional:**

| Argument    | Type              | Description                                                                 |
|-------------|-------------------|-----------------------------------------------------------------------------|
| `expiresIn` | <pre>TimeSpan?</pre> | Set the token expiration duration (default: depends on Kuzzle configuration file) |

## Return

A JObject with the following properties:

| Property    | Type              | Description                                                                              |
|-------------|-------------------|------------------------------------------------------------------------------------------|
| `_id`       | <pre>string</pre> | User's `kuid`                                                                            |
| `jwt`       | <pre>string</pre> | Encrypted authentication token, that must then be sent in the requests headers or in the query |
| `expiresAt` | <pre>Int64</pre>  | Token expiration date, in Epoch-millis (UTC)                                             |
| `ttl`       | <pre>Int64</pre>  | Token time to live, in milliseconds                                                      |

Once `auth:refreshToken` has been called, the returned authentication token is stored by the SDK and used for all the subsequent API call, ensuring they are properly authenticated.

## Usage

<<< ./snippets/refresh-token.cs

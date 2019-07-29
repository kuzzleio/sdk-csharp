---
code: true
type: page
title: refreshToken
description: Refreshes an authentication token.
---

# checkToken

Refreshes an authentication token.

- a valid, non-expired authentication must be provided
- the provided authentication token is revoked
- a new authentication token is generated and returned


## Signature

```csharp
public async Task<JObject> RefreshTokenAsync(string expiresIn = null);
```

## Arguments

**Optional:**

| Arguments   | Type              | Description                                                                 |
|-------------|-------------------|-----------------------------------------------------------------------------|
| `expiresIn` | <pre>string</pre> | Set the expiration duration (default: depends on Kuzzle configuration file) |

## Return

A JObject which has the following properties:

| Property    | Type              | Description                                                                              |
|-------------|-------------------|------------------------------------------------------------------------------------------|
| `_id`       | <pre>string</pre> | User's `kuid`                                                                            |
| `jwt`       | <pre>string</pre> | Encrypted JSON Web Token, that must then be sent in the requests headers or in the query |
| `expiresAt` | <pre>Int64</pre>  | Token expiration date, in Epoch-millis (UTC)                                             |
| `ttl`       | <pre>Int64</pre>  | Token time to live, in milliseconds                                                      |

Once `auth:refreshToken` has been called, the returned JWT is stored by the SDK and used for all the subsequent API call, ensuring they are properly authenticated.

## Usage

<<< ./snippets/refresh-token.cs
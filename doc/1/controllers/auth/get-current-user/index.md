---
code: true
type: page
title: getCurrentUser
description: Returns the profile object for the user linked to the `JSON Web Token`
---

# getCurrentUser

Returns informations about the user currently loggued with the SDK instance.

## Signature

```csharp
public async Task<JObject> GetCurrentUserAsync();
```

## Return

A JObject representing the User.

| Property     | Type               | Description                                       |
|--------------|--------------------|---------------------------------------------------|
| `_id`        | <pre>string</pre>  | Representing the current user `kuid`              |
| `strategies` | <pre>JArray</pre>  | Available authentication strategies for that user |
| `_source`    | <pre>JObject</pre> | User information                                  |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/get-current-user.cs

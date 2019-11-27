---
code: true
type: page
title: UpdateSelfAsync
description: Updates the current user object in Kuzzle.
---

# UpdateSelfAsync

Updates the current user object in Kuzzle.

## Arguments

```csharp
public async Task<JObject> UpdateSelfAsync(JObject content);
```

| Argument  | Type               | Description                           |
|-----------|--------------------|---------------------------------------|
| `content` | <pre>JObject</pre> | JObject representing the user content |

## Return

Return a JObject with the following properties:

| Property  | Type               | Description                               |
|-----------|--------------------|-------------------------------------------|
| `_id`     | <pre>string</pre>  | User's `kuid`                             |
| `_source` | <pre>JObject</pre> | Additional (and optional) user properties |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/update-self.cs

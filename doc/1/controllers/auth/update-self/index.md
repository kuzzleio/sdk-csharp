---
code: true
type: page
title: updateSelf
description: Updates the current user object in Kuzzle.
---

# updateSelf

Updates the current user object in Kuzzle.

## Signature

```csharp
public async Task<JObject> UpdateSelfAsync(JObject content);
```

## Arguments

| Arguments | Type               | Description                           |
|-----------|--------------------|---------------------------------------|
| `content` | <pre>JObject</pre> | JObject representing the user content |

## Return

Return a JObject with the following properties:

| Property  | Type               | Description                               |
|-----------|--------------------|-------------------------------------------|
| `_id`     | <pre>string</pre>  | User's `kuid`                             |
| `_source` | <pre>JObject</pre> | Additional (and optional) user properties |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/update-self.cs

---
code: true
type: page
title: CheckRightsAsync
description: Checks if the provided API request can be executed by this network connection, using the current authentication information.
---

# CheckRightsAsync

Checks if the provided API request can be executed by this network connection, using the current authentication information.

## Arguments

```csharp
public async Task<bool> CheckRightsAsync(JObject requestPayload);
```

| Argument         | Type               | Description                                                                |
| ---------------- | ------------------ | -------------------------------------------------------------------------- |
| `requestPayload` | <pre>JObject</pre> | A JSON Object containing at least the `controller` and `action` properties |

## Return

A boolean telling whether the provided request would have been allowed or not

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).


## Usage

<<< ./snippets/check-rigths.cs

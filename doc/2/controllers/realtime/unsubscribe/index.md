---
code: true
type: page
title: UnsubscribeAsync
description: Removes a subscription.
---

# UnsubscribeAsync

Removes a subscription.

## Arguments

```csharp
public async Task UnsubscribeAsync(string roomId);
```

| Argument  | Type               | Description          |
|-----------|--------------------|----------------------|
| `room_id` | <pre>string&</pre> | Subscription room ID |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/unsubscribe.cs

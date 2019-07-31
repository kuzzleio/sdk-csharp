---
code: true
type: page
title: UnsubscribeAsync
description: Removes a subscription
---

# UnsubscribeAsync

Removes a subscription.

## Signature

```csharp
public async Task UnsubscribeAsync(string roomId);
```

## Arguments

| Arguments | Type                          | Description          |
|-----------|-------------------------------|----------------------|
| `room_id` | <pre>string&</pre> | Subscription room ID |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/unsubscribe.cs
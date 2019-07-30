---
code: true
type: page
title: unsubscribe
description: Removes a subscription
---

# unsubscribe

Removes a subscription.

## Signature

```csharp
public async Task UnsubscribeAsync(string roomId);
```

## Arguments

| Arguments | Type                          | Description          |
|-----------|-------------------------------|----------------------|
| `room_id` | <pre>const std::string&</pre> | Subscription room ID |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/unsubscribe.cs

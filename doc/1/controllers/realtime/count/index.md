---
code: true
type: page
title: CountAsync
description: Returns the number of other connections sharing the same subscription.
---

# CountAsync

Returns the number of other connections sharing the same subscription.

## Arguments

```csharp
public async Task<int> CountAsync(string roomId);
```

| Argument  | Type              | Description          |
|-----------|-------------------|----------------------|
| `room_id` | <pre>string</pre> | Subscription room ID |

## Return

Returns the number of active connections using the same provided subscription room.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/count.cs

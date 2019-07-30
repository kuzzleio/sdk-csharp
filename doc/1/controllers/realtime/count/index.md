---
code: true
type: page
title: count
description: Count subscribers for a subscription room
---

# count

Count the number of subscribers for a subscription room

## Signature

```csharp
public async Task<int> CountAsync(string roomId);
```

## Arguments

| Arguments | Type              | Description          |
|-----------|-------------------|----------------------|
| `room_id` | <pre>string</pre> | Subscription room ID |

## Return

Returns the number of active connections using the same provided subscription room.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/count.cs

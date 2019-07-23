---
code: true
type: page
title: GetStatsAsync
description: Returns statistics snapshots within a provided timestamp range.
---

# GetStatsAsync

Returns statistics snapshots within a provided timestamp range.
By default, snapshots are made every 10 seconds and they are stored for 1 hour.

These statistics include:

- the number of connected users per protocol (not available for all protocols)
- the number of ongoing requests
- the number of completed requests since the last frame
- the number of failed requests since the last frame

## Signature

```csharp
async Task<JObject> GetStatsAsync(Int64 start, Int64 end)
```

## Arguments

| Arguments | Type | Description                      | Required |
| --------- | ---- | -------------------------------- | -------- |
| `start`   | long | begining of statistics frame set | yes      |
| `end`     | long | end of statistics frame set      | yes      |

## Return

Return statistics snapshots within a provided timestamp range as a `JObject`.

## Usage

<<< ./snippets/getStats.cs

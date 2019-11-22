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

## Arguments

```csharp
async Task<JObject> GetStatsAsync(Int64 start, Int64 end)
```

| Argument | Type            | Description                      |
|----------|-----------------|----------------------------------|
| `start`  | <pre>long</pre> | beginning of statistics frame set |
| `end`    | <pre>long</pre> | end of statistics frame set      |

## Return

Returns statistics snapshots within a provided timestamp range as a `JObject`.

## Usage

<<< ./snippets/getStats.cs

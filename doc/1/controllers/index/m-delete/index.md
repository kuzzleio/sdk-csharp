---
code: true
type: page
title: MDeleteAsync
description: Deletes multiple indexes.
---

# MDeleteAsync

Deletes multiple indexes.

## Arguments

```cs
Task<JArray> MDeleteAsync(JArray indexes);
```

| Argument  | Type                 | Description           |
| --------- | -------------------- | --------------------- |
| `indexes` | JArray               | List of indexes names |

## Return

Returns a `JArray` containing the list of indexes names deleted.

## Usage

<<< ./snippets/mDelete.cs

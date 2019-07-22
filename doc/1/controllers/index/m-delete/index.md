---
code: true
type: page
title: mDelete
description: Deletes multiple indexes
---

# mDelete

Deletes multiple indexes at once.

## Signature

```cs
Task<JArray> MDeleteAsync(JArray indexes);
```

## Arguments

| Arguments | Type                 | Description           | Required |
| --------- | -------------------- | --------------------- | -------- |
| `indexes` | JArray               | List of indexes names | yes      |

## Return

Returns a `JArray` containing the list of indexes names deleted.

## Usage

<<< ./snippets/mDelete.cs

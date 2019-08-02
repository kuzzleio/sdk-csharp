---
code: true
type: page
title: ListAsync
description: List the indexes.
---

# ListAsync

Get the complete list of indexes handled by Kuzzle.

## Arguments

```cs
Task<JArray> ListAsync();
```

## Return

Returns an `JArray` containing the list of indexes names present in Kuzzle.

## Usage

<<< ./snippets/list.cs

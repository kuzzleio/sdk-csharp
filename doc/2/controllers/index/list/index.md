---
code: true
type: page
title: ListAsync
description: List the indexes.
---

# ListAsync

Gets the complete list of indexes handled by Kuzzle.

## Arguments

```csharp
Task<JArray> ListAsync();
```

## Return

Returns a `JArray` containing the list of index names handled by Kuzzle.

## Usage

<<< ./snippets/list.cs

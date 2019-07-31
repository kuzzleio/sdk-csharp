---
code: true
type: page
title: ExistsAsync
description: Check for index existence
---

# ExistsAsync

Checks if the given index exists in Kuzzle.

## Arguments

```cs
Task<bool> ExistsAsync(string index);
```

| Arguments | Type                       | Description       |
| --------- | -------------------------- | ----------------- |
| `index`   | string                     | Index name        |

## Return

Returns a `bool` that indicate whether the index exists or not.

## Usage

<<< ./snippets/exists.cs

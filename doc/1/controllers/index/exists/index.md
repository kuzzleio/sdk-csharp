---
code: true
type: page
title: exists
description: Check for index existence
---

# Exists

Checks if the given index exists in Kuzzle.

## Signature

```cs
Task<bool> ExistsAsync(string index);
```

## Arguments

| Arguments | Type                       | Description       | Required |
| --------- | -------------------------- | ----------------- | -------- |
| `index`   | string                     | Index name        | yes      |

## Return

Returns a `bool` that indicate whether the index exists or not.

## Usage

<<< ./snippets/exists.cs

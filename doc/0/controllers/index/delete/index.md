---
code: true
type: page
title: delete
description: Deletes an index
---

# Delete

Deletes an entire index from Kuzzle.

## Signature

```cs
Task DeleteAsync(string index);
```

## Arguments

| Arguments | Type                       | Description       | Required |
| --------- | -------------------------- | ----------------- | -------- |
| `index`   | string                     | Index name        | yes      |

## Usage

<<< ./snippets/delete.cs

---
code: true
type: page
title: ExistsAsync
description: Check for index existence.
---

# ExistsAsync

Checks if the given index exists in Kuzzle.

## Arguments

```csharp
Task<bool> ExistsAsync(string index);
```

| Argument | Type              | Description |
|----------|-------------------|-------------|
| `index`  | <pre>string</pre> | Index name  |

## Return

Returns a `bool` that indicates whether the index exists or not.

## Usage

<<< ./snippets/exists.cs

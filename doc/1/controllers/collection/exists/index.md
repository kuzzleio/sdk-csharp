---
code: true
type: page
title: ExistsAsync
description: Check if collection exists
---

# ExistsAsync

Check if a collection exists in Kuzzle.

## Signature

```csharp
public async Task<bool> ExistsAsync(
    string index,
    string collection);
```

## Arguments

| Arguments    | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Return

A boolean indicating if the collection exists or not.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/exists.cs

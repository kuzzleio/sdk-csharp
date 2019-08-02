---
code: true
type: page
title: MGetAsync
description: Gets multiple documents from kuzzle.
---

# MGetAsync

Gets multiple documents.

Throws a partial error (error code 206) if one or more document can not be retrieved.

## Arguments

```csharp
public async Task<JArray> MGetAsync(
  string index, 
  string collection, 
  JArray ids);

```

<br/>

| Argument     | Type                                      | Description     |
| ------------ | ----------------------------------------- | --------------- |
| `index`      | <pre>string</pre>             | Index name      |
| `collection` | <pre>string</pre>             | Collection name |
| `ids`        | <pre>JArray</pre> | Document IDs    |

## Return

A JArray containing the retrieved documents.

Each document has the following properties:

| Property   | Type              | Description      |
| ---------- | ----------------- | ---------------- |
| `_id`     | <pre>string</pre> | Document ID      |
| `_source` | <pre>JObject</pre> | Document content |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/m-get.cs

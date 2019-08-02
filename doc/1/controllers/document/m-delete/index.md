---
code: true
type: page
title: MDeleteAsync
description: Deletes multiple documents
---

# MDeleteAsync

Deletes multiple documents.

Throws a partial error (error code 206) if one or more document deletions fail.

## Signature

```csharp
public async Task<JArray> MDeleteAsync(
  string index, 
  string collection, 
  JArray ids, 
  bool waitForRefresh = false);

```

## Arguments

| Argument     | Type                                      | Description                    |
| ------------ | ----------------------------------------- | ------------------------------ |
| `index`      | <pre>string</pre>             | Index name                     |
| `collection` | <pre>string</pre>             | Collection name                |
| `ids`        | <pre>JArray</pre> | IDs of the documents to delete |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

## Return

A JArray containing the deleted documents IDs.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/m-delete.cs

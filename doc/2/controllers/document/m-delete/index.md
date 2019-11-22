---
code: true
type: page
title: MDeleteAsync
description: Deletes multiple documents.
---

# MDeleteAsync

Deletes multiple documents.

Throws a partial error (error code 206) if one or more document deletions fail.

## Arguments

```csharp
public async Task<string[]> MDeleteAsync(
  string index, 
  string collection, 
  string[] ids, 
  bool waitForRefresh = false);

```

<br/>

| Argument     | Type                                      | Description                    |
| ------------ | ----------------------------------------- | ------------------------------ |
| `index`      | <pre>string</pre>             | Index name                     |
| `collection` | <pre>string</pre>             | Collection name                |
| `ids`        | <pre>string[]</pre> | IDs of the documents to delete |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

## Return

An array of strings containing the deleted document IDs.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/m-delete.cs

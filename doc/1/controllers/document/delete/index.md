---
code: true
type: page
title: DeleteAsync
description: Deletes a document from kuzzle.
---

# DeleteAsync

Deletes a document.

## Arguments

```csharp
public async Task<string> DeleteAsync(
  string index, 
  string collection, 
  string id, 
  bool waitForRefresh = false);

```

<br/>

| Argument     | Type                                 | Description     |
| ------------ | ------------------------------------ | --------------- |
| `index`      | <pre>string</pre>        | Index name      |
| `collection` | <pre>string</pre>        | Collection name |
| `id`         | <pre>string</pre>        | Document ID     |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

## Return

The ID of the deleted document.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/delete.cs

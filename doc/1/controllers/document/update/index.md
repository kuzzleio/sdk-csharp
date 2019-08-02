---
code: true
type: page
title: UpdateAsync
description: Updates a document
---

# UpdateAsync

Updates a document content.

Conflicts may occur if the same document gets updated multiple times within a short timespan, in a database cluster.  
You can set the `retryOnConflict` optional argument (with a retry count), to tell Kuzzle to retry the failing updates the specified amount of times before rejecting the request with an error.

## Arguments

```csharp
public async Task<JObject> UpdateAsync(
  string index,
  string collection,
  string id,
  JObject changes,
  bool waitForRefresh = false,
  int retryOnConflict = 0);

```

<br/>

| Argument     | Type                                 | Description                           |
| ------------ | ------------------------------------ | ------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                            |
| `collection` | <pre>string</pre>        | Collection name                       |
| `id`         | <pre>string</pre>        | Document ID                           |
| `changes`   | <pre>JObject</pre>        | JObject representing the modified fields |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |
| `retryOnConflict` | <pre>int</pre><br/>(`0`)                 | The number of times the database layer should retry in case of version conflict    |

## Return

A JObject representing an object containing the document creation result.

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `result`    | <pre>string</pre> | Set to `updated` in case of success                    |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/update.cs

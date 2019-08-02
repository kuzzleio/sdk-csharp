---
code: true
type: page
title: MUpdateAsync
description: Updates documents
---

# MUpdateAsync

Updates multiple documents.

Returns a partial error (error code 206) if one or more documents can not be updated.

Conflicts may occur if the same document gets updated multiple times within a short timespan in a database cluster.

You can set the `retryOnConflict` optional argument (with a retry count), to tell Kuzzle to retry the failing updates the specified amount of times before rejecting the request with an error.

## Arguments

```csharp
public async Task<JArray> MUpdateAsync(
  string index, 
  string collection, 
  JArray documents, 
  bool waitForRefresh = false, 
  int retryOnConflict = 0);

```

<br/>

| Argument     | Type                                 | Description                                      |
| ------------ | ------------------------------------ | ------------------------------------------------ |
| `index`      | <pre>string</pre>        | Index name                                       |
| `collection` | <pre>string</pre>        | Collection name                                  |
| `documents`  | <pre>JObject</pre>        | JObject representing the documents to update |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |
| `retryOnConflict` | <pre>int</pre><br/>(`0`)                 | The number of times the database layer should retry in case of version conflict    |

### documents

Each document has the following properties:

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | Document ID      |
| `body` | <pre>JObject</pre> | Document body |

## Return

A JArray representing the replaced documents.  

Each document has the following properties:

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | JObject representing the created document          |
| `result`    | <pre>string</pre> | Set to `updated`.                    |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/m-update.cs

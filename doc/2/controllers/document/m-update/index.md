---
code: true
type: page
title: MUpdateAsync
description: Updates documents.
---

# MUpdateAsync

Updates multiple documents.

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
| `documents`  | <pre>JArray</pre>        | JArray of documents to update |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |
| `retryOnConflict` | <pre>int</pre><br/>(`0`)                 | The number of times the database layer should retry in case of version conflict    |

### documents

Each document has the following properties:

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | Document ID      |
| `body` | <pre>JObject</pre> | Document body |

## Return

Returns a JObject containing 2 arrays: `successes` and `errors`

Successful document updates are returned in the `successes` array as objects with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | Document ID                     |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | Document content                                       |

Failed updated are returned in the `errors` array as objects with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `document`  | <pre>JObject</pre> | Failed update                                      |
| `status` | <pre>int</pre> | HTTP error status |
| `reason`  | <pre>string</pre> | Human readable reason |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/m-update.cs

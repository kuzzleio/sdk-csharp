---
code: true
type: page
title: WriteAsync
desription: Create or replace a document directly into the storage engine.
---

# WriteAsync

Create or replace a document directly into the storage engine.

## Signature

```csharp
async Task<JObject> WriteAsync(
    string index,
    string collection,
    JObject documentContent,
    string documentId = null,
    bool waitForRefresh = false,
    bool notify = false
)
```

## Arguments

| Arguments         | Type               | Description                 |
| ----------------- | ------------------ | --------------------------- |
| `index`           | <pre>string</pre>  | Index name                  |
| `collection`      | <pre>string</pre>  | Collection name             |
| `documentContent` | <pre>JObject</pre> | Document content to create. |

### Options

| Property         | Type              | Description                                                                              |
| ---------------- | ----------------- | ---------------------------------------------------------------------------------------- |
| `documentId`     | <pre>string</pre> | set the document unique ID to the provided value, instead of auto-generating a random ID |
| `waitForRefresh` | <pre>bool</pre>   | If set to true, Kuzzle will not respond until the created/replaced documents are indexed |
| `notify`         | <pre>bool</pre>   | If set to true, Kuzzle will trigger realtime notifications                               |

## Return

A `hits` array, containing the list of created documents, in the same order than the one provided in the query.

| Property   | Type               | Description                                     |
| ---------- | ------------------ | ----------------------------------------------- |
| `_id`      | <pre>string</pre>  | Created document unique identifier.             |
| `_source`  | <pre>JObject</pre> | Document content.                               |
| `_version` | <pre>int</pre>     | Version number of the document                  |
| `created`  | <pre>bool</pre>    | A boolean telling whether a document is created |

## Usage

<<< ./snippets/write-async.cs


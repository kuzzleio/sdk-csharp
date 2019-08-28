---
code: true
type: page
title: WriteAsync
description: Create or replace a document directly into the storage engine.
---

# WriteAsync

Create or replace a document directly into the storage engine.

## Arguments

```csharp
public async Task<JObject> WriteAsync(
    string index,
    string collection,
    JObject content,
    string documentId = null,
    bool waitForRefresh = false,
    bool notify = false);
```

| Argument     | Type               | Description                 |
|--------------|--------------------|-----------------------------|
| `index`      | <pre>string</pre>  | Index name                  |
| `collection` | <pre>string</pre>  | Collection name             |
| `content`    | <pre>JObject</pre> | Document content to create. |

### Options

| Property         | Type                          | Description                                                                              |
|------------------|-------------------------------|------------------------------------------------------------------------------------------|
| `documentId`     | <pre>string</pre><br>(`null`) | set the document unique ID to the provided value, instead of auto-generating a random ID |
| `waitForRefresh` | <pre>bool</pre><br>(`false`)  | If set to true, Kuzzle will not respond until the created/replaced documents are indexed |
| `notify`         | <pre>bool</pre><br>(`false`)  | If set to true, Kuzzle will trigger realtime notifications                               |

## Return

Return a JObject with the following properties:

| Property   | Type               | Description                                     |
|------------|--------------------|-------------------------------------------------|
| `_id`      | <pre>string</pre>  | Created document unique identifier.             |
| `_source`  | <pre>JObject</pre> | Document content.                               |
| `_version` | <pre>int</pre>     | Version number of the document                  |
| `created`  | <pre>bool</pre>    | A boolean telling whether a document is created |

## Usage

<<< ./snippets/write-async.cs

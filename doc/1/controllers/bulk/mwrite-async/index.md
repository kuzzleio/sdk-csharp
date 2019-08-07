---
code: true
type: page
title: MWriteAsync
description: Create or replace multiple documents directly into the storage engine.
---

# MWriteAsync

This is a low level route intended to bypass Kuzzle actions on document creation, notably:

- check document validity
- add kuzzle metadata
- trigger realtime notifications (unless asked otherwise)

## Arguments

```csharp
public async Task<JObject> MWriteAsync(
    string index,
    string collection,
    JArray documents,
    bool waitForRefresh = false,
    bool notify = false);
```

| Argument     | Type              | Description                                                                                                                      |
|--------------|-------------------|----------------------------------------------------------------------------------------------------------------------------------|
| `index`      | <pre>string</pre> | Index name                                                                                                                       |
| `collection` | <pre>string</pre> | Collection name                                                                                                                  |
| `documents`  | <pre>JArray</pre> | An array of JObject. Each JObject describes a document to create or replace, by exposing the following properties: `_id`, `body` |

### Options

| Property         | Type                         | Description                                                                              |
|------------------|------------------------------|------------------------------------------------------------------------------------------|
| `waitForRefresh` | <pre>bool</pre><br>(`false`) | If set to true, Kuzzle will not respond until the created/replaced documents are indexed |
| `notify`         | <pre>bool</pre><br>(`false`) | If set to true, Kuzzle will trigger realtime notifications                               |

## Return

A `JObject` containing a `hits` array which represent the list of created documents, in the same order than the one provided in the query.

| Property   | Type               | Description                                     |
|------------|--------------------|-------------------------------------------------|
| `_id`      | <pre>string</pre>  | Created document unique identifier.             |
| `_source`  | <pre>JObject</pre> | Document content.                               |
| `_version` | <pre>int</pre>     | Version number of the document                  |
| `created`  | <pre>bool</pre>    | A boolean telling whether a document is created |

## Usage

<<< ./snippets/mwrite-async.cs


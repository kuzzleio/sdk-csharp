---
code: true
type: page
title: MWriteAsync
description: Creates or replaces multiple documents directly into the storage engine.
---

# MWriteAsync

This is a low level route intended to bypass Kuzzle actions on document creation, notably:
  - check [document validity](/core/2/guides/advanced/data-validation),
  - add [kuzzle metadata](/core/2/guides/main-concepts/data-storage#kuzzle-metadata),
  - trigger [realtime notifications](/core/2/guides/main-concepts/realtime-engine) (unless asked otherwise)

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
| `documents`  | <pre>JArray</pre> | An array of JObject representing the documents|

### documents

An array of `JObject`. Each object describes a document to create or replace, by exposing the following properties:
  - `_id`: document unique identifier (optional)
  - `body`: document content

### Options

| Property         | Type                         | Description                                                                              |
|------------------|------------------------------|------------------------------------------------------------------------------------------|
| `waitForRefresh` | <pre>bool</pre><br>(`false`) | If set to true, Kuzzle will not respond until the created/replaced documents are indexed |
| `notify`         | <pre>bool</pre><br>(`false`) | If set to true, Kuzzle will trigger realtime notifications                               |

## Return

Returns a `JObject` containing 2 arrays: `successes` and `errors`

Each created or replaced document is an object of the `successes` array with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>String</pre> | Document ID                     |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | Document content                                       |
| `created`  | <pre>bool</pre> | True if the document was created |

Each errored document is an object of the `errors` array with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `document`  | <pre>JObject</pre> | Document that cause the error                                       |
| `status` | <pre>int</pre> | HTTP error status |
| `reason`  | <pre>String</pre> | Human readable reason |

## Usage

<<< ./snippets/mwrite-async.cs

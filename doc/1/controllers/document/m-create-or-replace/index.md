---
code: true
type: page
title: MCreateOrReplaceAsync
description: Create or replace documents in kuzzle
---

# MCreateOrReplaceAsync

Creates or replaces multiple documents.

Throws a partial error (error code 206) if one or more document creations/replacements fail.

## Signature

```csharp
public async Task<JArray> MCreateOrReplaceAsync(
  string index, 
  string collection, 
  JArray documents, 
  bool waitForRefresh = false);

```

## Arguments

| Argument     | Type                                 | Description                                      |
| ------------ | ------------------------------------ | ------------------------------------------------ |
| `index`      | <pre>string</pre>        | Index name                                       |
| `collection` | <pre>string</pre>        | Collection name                                  |
| `documents`       | <pre>JArray</pre>        | A JArray containing the documents to create |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

### documents

Each document has the following properties:

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | Document ID      |
| `body` | <pre>JObject</pre> | Document body |

## Return

A JArray representing the created documents.  

Each document has the following properties:

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | JObject representing the created document          |
| `result`    | <pre>string</pre> | Set to `created` or `replaced`.                    |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/m-create-or-replace.cs

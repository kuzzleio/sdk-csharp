---
code: true
type: page
title: CreateOrReplaceAsync
description: Creates or replaces a document.
---

# CreateOrReplaceAsync

Creates a new document in the persistent data storage, or replaces its content if it already exists.

## Arguments

```csharp
public async Task<JObject> CreateOrReplaceAsync( 
  string index, 
  string collection, 
  string id, 
  JObject content, 
  bool waitForRefresh = false);

```

<br/>

| Argument     | Type                                 | Description                                       |
| ------------ | ------------------------------------ | ------------------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                                                  |
| `collection` | <pre>string</pre>        | Collection name                                             |
| `id`         | <pre>string</pre><br/>       | Document ID |
| `content`   | <pre>JObject</pre>        | JObject representing the body of the document           |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

## Return

A JObject containing the document creation result.

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | JObject representing the created document          |
| `result`    | <pre>string</pre> | Set to `created` or `replaced`.                    |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/create-or-replace.cs

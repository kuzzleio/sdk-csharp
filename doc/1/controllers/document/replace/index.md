---
code: true
type: page
title: ReplaceAsync
description: Replaces a document
---

# ReplaceAsync

Replaces the content of an existing document.

## Arguments

```csharp
public async Task<JObject> ReplaceAsync(
  string index, string collection, string id, JObject content, bool waitForRefresh = false );

```

<br/>

| Argument     | Type                                 | Description                           |
| ------------ | ------------------------------------ | ------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                            |
| `collection` | <pre>string</pre>        | Collection name                       |
| `id`         | <pre>string</pre>        | Document ID                           |
| `document`   | <pre>JObject</pre>        | JObject representing the document |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |


## Return

A JObject representing an object containing the document creation result.

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | JObject representing the replaced document         |
| `result`    | <pre>string</pre> | Set to `replaced` in case of success                   |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/replace.cs

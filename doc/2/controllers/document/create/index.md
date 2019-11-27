---
code: true
type: page
title: CreateAsync
description: Creates a new document.
---

# CreateAsync

Creates a new document in the persistent data storage.

Throws an error if an ID is provided and if a document with that ID already exists.

## Arguments

```csharp
public async Task<JObject> CreateAsync( 
  string index, 
  string collection, 
  JObject content, 
  string id = null, 
  bool waitForRefresh = false);

```

<br/>

| Option     | Type<br/>(default)                       | Description                                                                        |
| ------------ | ------------------------------------ | ----------------------------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                                                  |
| `collection` | <pre>string</pre>        | Collection name                                             |
| `content`   | <pre>JObject</pre>        | JObject representing the body of the document           |
| `id`         | <pre>string</pre><br/>(`null`)        | Document ID. Will use an auto-generated ID if not specified |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)       | If `true`, waits for the change to be reflected for `search` (up to 1s)           |

## Return

A JObject containing the document creation result.

| Property  | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>string</pre> | ID of the newly created document                       |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | JObject representing the created document          |
| `result`    | <pre>string</pre> | Set to `created` in case of success                    |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/create.cs

---
code: true
type: page
title: MCreateOrReplaceAsync
description: Creates or replaces documents in kuzzle.
---

# MCreateOrReplaceAsync

Creates or replaces multiple documents.

## Arguments

```csharp
public async Task<JObject> MCreateOrReplaceAsync(
  string index, 
  string collection, 
  JArray documents, 
  bool waitForRefresh = false);

```

<br/>

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

Returns a `JObject` object with 2 array properties: `successes` and `errors`

Successful document creations/replacements are returned in the `successes` array as objects with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>String</pre> | Document ID                     |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | Document content                                       |
| `result`    | <pre>String</pre> | Set to `created` or `replaced`.                    |

Failed document imports are returned in the `errors` array as objects with the following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `document`  | <pre>JObject</pre> | Failed document                                       |
| `status` | <pre>int</pre> | HTTP error status |
| `reason`  | <pre>String</pre> | Human readable reason |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/m-create-or-replace.cs

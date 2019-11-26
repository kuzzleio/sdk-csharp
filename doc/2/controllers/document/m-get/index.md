---
code: true
type: page
title: MGetAsync
description: Gets multiple documents from kuzzle.
---

# MGetAsync

Gets multiple documents.

## Arguments

```csharp
public async Task<JArray> MGetAsync(
  string index, 
  string collection, 
  JArray ids);

```

<br/>

| Argument     | Type                                      | Description     |
| ------------ | ----------------------------------------- | --------------- |
| `index`      | <pre>string</pre>             | Index name      |
| `collection` | <pre>string</pre>             | Collection name |
| `ids`        | <pre>JArray</pre> | Document IDs    |

## Return

Returns a JObject containing 2 arrays: `successes` and `errors`

The `successes` array contain the list of retrieved documents.

Each document have with following properties:

| Name      | Type              | Description                                            |
| --------- | ----------------- | ------------------------------------------------------ |
| `_id`      | <pre>String</pre> | Document ID                    |
| `_version` | <pre>int</pre> | Version of the document in the persistent data storage |
| `_source`  | <pre>JObject</pre> | Document content                                       |

The `errors` array contain the IDs of not found documents.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/m-get.cs

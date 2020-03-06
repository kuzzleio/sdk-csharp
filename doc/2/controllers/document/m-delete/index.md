---
code: true
type: page
title: MDeleteAsync
description: Deletes multiple documents.
---

# MDeleteAsync

Deletes multiple documents.

## Arguments

```csharp
public async Task<JObject> MDeleteAsync(
  string index, 
  string collection, 
  string[] ids, 
  bool waitForRefresh = false);

```

<br/>

| Argument           | Type                                      | Description                    |
| ------------------ | ----------------------------------------- | ------------------------------ |
| `index`            | <pre>string</pre>                         | Index name                     |
| `collection`       | <pre>string</pre>                         | Collection name                |
| `ids`              | <pre>string[]</pre>                       | IDs of the documents to delete |
| `waitForRefresh`   | <pre>bool</pre><br/>(`false`)             | If `true`, waits for the change to be reflected for `search` (up to 1s) |

## Return

Returns a JObject containing 2 arrays: `successes` and `errors`

The `successes` array contains the successfully deleted document IDs.

Each deletion error is an object of the `errors` array with the following properties:

| Name      | Type              | Description           |
| --------- | ----------------- | --------------------- |
| `id`      | <pre>String</pre> | Document ID           |
| `reason`  | <pre>String</pre> | Human readable reason |
| `status`  | <pre>int</pre>    | HTTP status code      |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/m-delete.cs

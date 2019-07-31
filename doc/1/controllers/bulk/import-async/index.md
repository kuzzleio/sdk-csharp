---
code: true
type: page
title: ImportAsync
desription: Creates, updates or deletes large amounts of documents as fast as possible.
---

# ImportAsync

Creates, updates or deletes large amounts of documents as fast as possible.

This route is faster than the document:m* routes family (e.g. document:mCreate), but no real-time notifications will be generated, even if some of the documents in the import match subscription filters.

If some documents actions fail, the client will receive a PartialError error.

## Signature

```csharp
public async Task<JObject> ImportAsync(
    string index,
    string collection,
    JArray bulkData);
```

## Arguments

| Arguments    | Type              | Description                                                  |
| ------------ | ----------------- | ------------------------------------------------------------ |
| `index`      | <pre>string</pre> | Index name                                                   |
| `collection` | <pre>string</pre> | Collection name                                              |
| `bulkData`   | <pre>JArray</pre> | Bulk operations to perform, following ElasticSearch Bulk API |

## Return

A JObject containing information about the import status for each document.

| Property | Type              | Description                                                                                |
| -------- | ----------------- | ------------------------------------------------------------------------------------------ |
| `errors` | <pre>bool</pre>   | Boolean indicating if some error occured during the import.                                |
| `items`  | <pre>JArray</pre> | Array containing the list of executed queries result, in the same order than in the query. |

**Notes:** Each object has the following properties:

| Property | Type              | Description                                                                                |
| -------- | ----------------- | ------------------------------------------------------------------------------------------ |
| `_id`    | <pre>string</pre> | Document unique identifier.                                                                |
| `status` | <pre>int</pre>    | HTTP status code for that query. `201` (created) or `206` (partial error).                 |

## Usage

<<< ./snippets/import-async.cs
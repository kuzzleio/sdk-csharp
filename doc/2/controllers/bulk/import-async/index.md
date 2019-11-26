---
code: true
type: page
title: ImportAsync
description: Creates, updates or deletes large amounts of documents as fast as possible.
---

# ImportAsync

Creates, updates or deletes large amounts of documents as fast as possible.

This route is faster than the `document:m*` routes family (e.g. [document:mCreate](/sdk/csharp/2/controllers/document/m-create)), but no real-time notifications will be generated, even if some of the documents in the import match subscription filters.

## Arguments

```csharp
public async Task<JObject> ImportAsync(
    string index,
    string collection,
    JArray bulkData);
```

| Argument     | Type              | Description                                                  |
|--------------|-------------------|--------------------------------------------------------------|
| `index`      | <pre>string</pre> | Index name                                                   |
| `collection` | <pre>string</pre> | Collection name                                              |
| `bulkData`   | <pre>JArray</pre> | Bulk operations to perform, following ElasticSearch Bulk API |

### bulkData

This API takes a JSON array containing a list of objects working in pairs.
In each pair, the first object specifies the action to perform (the most common is `create`) and the second specifies the document itself, like in the example below:

```json
[
  // Action object
  { "create": { "_id": "id" } },
  // Document object
  { "myField": "myValue", "myOtherField": "myOtherValue" },

  // Another action object
  { "create": { "_id": "another-id" } },
  // Another document object
  { "myField": "anotherValue", "myOtherField": "yetAnotherValue" }
];
```

Possible actions are `create`, `index`, `update`, `delete`.

Learn more at [Elasticsearch Bulk API](https://www.elastic.co/guide/en/elasticsearch/reference/7.4/docs-bulk.html)

## Return

A JObject containing 2 arrays:

| Property | Type                | Description                                         |
| -------- | ------------------- | --------------------------------------------------- |
| `successes`  | <pre>JArray</pre> | Array of object containing successful document import |
| `errors` | <pre>JArray</pre>  | Array of object containing failed document import     |

Each item of the `successes` array is an object containing the action name as key and the corresponding object contain the following properties:

| Property | Type                | Description                                         |
| -------- | ------------------- | --------------------------------------------------- |
| `_id`   | <pre>string</pre>   | Document unique identifier      |
| `status`   | <pre>int</pre>   | HTTP status code for that query      |

Each item of the `errors` array is an object containing the action name as key and the corresponding object contain the following properties:

| Property | Type                | Description                                         |
| -------- | ------------------- | --------------------------------------------------- |
| `_id`   | <pre>string</pre>   | Document unique identifier      |
| `status`   | <pre>int</pre>   | HTTP status code for that query      |
| `error`   | <pre>JObject</pre>   | Error object      |

Each `error` object contain the following properties:

| Property | Type                | Description                                         |
| -------- | ------------------- | --------------------------------------------------- |
| `type`  | <pre>string</pre> | Elasticsearch client error type |
| `reason`  | <pre>string</pre> | human readable error message |

## Usage

<<< ./snippets/import-async.cs
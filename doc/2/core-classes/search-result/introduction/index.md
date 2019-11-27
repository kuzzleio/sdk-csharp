---
code: false
type: page
title: Introduction
description: SearchResult class description and properties
order: 100
---

# SearchResult

This class represents a paginated search result.  

It can be returned by the following methods:
 - [Document.SearchAsync](/sdk/csharp/2/controllers/document/search)
 - [Collection.SearchSpecificationsAsync](/sdk/csharp/2/controllers/collection/search-specifications)

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk.API.DataObjects;
```

## Properties

| Property | Type | Description |
|--- |--- |--- |
| `Aggregations` | <pre>JObject</pre> | Search aggregations (can be undefined) |
| `Hits` | <pre>JArray</pre> | Page results |
| `Total` | <pre>int</pre> |  Total number of items that _can_ be retrieved |
| `Fetched` | <pre>int</pre> | Number of retrieved items so far |

### Hits

Each element of the `Hits` JArray is a JObject containing the following properties:

| Property | Type | Description |
|--- |--- |--- |
| `_id` | <pre>string</pre> | Document ID |
| `_score` | <pre>float</pre> | [Relevance score](https://www.elastic.co/guide/en/elasticsearch/guide/current/relevance-intro.html) |
| `_source` | <pre>JObject</pre> | Document content |

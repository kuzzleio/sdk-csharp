---
code: true
type: page
title: SearchOptions
description: SearchOptions class documentation
order: 110
---

# SearchOptions

This class represents the options usable with the search related methods.  

It can be used with the following methods:
 - [Document.SearchAsync](/sdk/csharp/2/controllers/document/search)
 - [Collection.SearchSpecificationsAsync](/sdk/csharp/2/controllers/collection/search-specifications)

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk.API.Options;
```

## Properties

| Property | Type | Description |
|--- |--- |--- |
| `From` | <pre>int</pre> | Offset of the first document to fetch |
| `Scroll` | <pre>string</pre> |  When set, gets a forward-only cursor having its ttl set to the given value (ie `30s`; cf [elasticsearch time limits](https://www.elastic.co/guide/en/elasticsearch/reference/7.3/common-options.html#time-units)) |
| `Size` | <pre>int</pre> | Maximum number of documents to retrieve per page |
| `Sort` | <pre>string</pre> | Field to sort the result on |
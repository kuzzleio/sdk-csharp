---
code: true
type: page
title: SearchAsync
description: Searches documents.
---

# SearchAsync

Searches documents.

There is a limit to how many documents can be returned by a single search query.
That limit is by default set at 10000 documents, and you can't get over it even with the from and size pagination options.

:::info
When processing a large number of documents (i.e. more than 1000), it is advised to paginate the results using [SearchResult.Next](/sdk/csharp/1/core-classes/search-result/next/) rather than increasing the size parameter.
:::

## Arguments

```csharp
public async Task<SearchResults> SearchAsync(
    string index, 
    string collection, 
    JObject query,
    SearchOptions options = null);
```

<br/>

| Argument     | Type                                 | Description                               |
| ------------ | ------------------------------------ | ----------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                                |
| `collection` | <pre>string</pre>        | Collection name                           |
| `query`      | <pre>JObject</pre>        | JObject representing the search query |
| `options`    | <pre>SearchOptions</pre> | Search options                             |

### query

A JObject representing the query. Query can have the following root properties:

- `query`: the search query itself, using the [ElasticSearch Query DSL](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/query-dsl.html) syntax.
- `aggregations`: control how the search results should be [aggregated](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/search-aggregations.html)
- `sort`: contains a list of fields, used to [sort search results](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/search-request-sort.html), in order of importance.

An empty body matches all documents in the queried collection.

### options

Additional search options.

| Option     | Type<br/>(default)                       | Description                         |
| ---------- | ---------------------------------------- | -------------------------------------------------------------------------------- |
| `From`     | <pre>int</pre><br/>(`0`)                 | Offset of the first document to fetch                                                                                                                                                                                 |
| `Size`     | <pre>int</pre><br/>(`10`)                | Maximum number of documents to retrieve per page                                                                                                                                                                      |
| `Scroll`   | <pre>string</pre><br/>(`""`) | When set, gets a forward-only cursor having its ttl set to the given value (ie `30s`; cf [elasticsearch time limits](https://www.elastic.co/guide/en/elasticsearch/reference/current/common-options.html#time-units)) |

## Return

Returns a SearchResult instance.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/search.cs

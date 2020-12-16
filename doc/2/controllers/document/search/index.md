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
When processing a large number of documents (i.e. more than 1000), it is advised to paginate the results using [SearchResults.Next](/sdk/csharp/2/core-classes/search-results/next) rather than increasing the size parameter.
:::

<SinceBadge version="change-me"/>

This method also supports the [Koncorde Filters DSL](/core/2/api/koncorde-filters-syntax) to match documents by passing the `lang` argument with the value `koncorde`.  
Koncorde filters will be translated into an Elasticsearch query.  

::: warning
Koncorde `bool` operator and `regexp` clause are not supported for search queries.
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
| `options`    | <pre>SearchOptions</pre> | An instance of [SearchOptions](/sdk/csharp/2/core-classes/search-options) class|

### query

A JObject representing the query. Query can have the following root properties:

- `query`: the search query itself, using the [ElasticSearch Query DSL](https://www.elastic.co/guide/en/elasticsearch/reference/7.4/query-dsl.html) or the [Koncorde Filters DSL](/core/2/api/koncorde-filters-syntax) syntax.
- `aggregations`: control how the search results should be [aggregated](https://www.elastic.co/guide/en/elasticsearch/reference/7.4/search-aggregations.html)
- `sort`: contains a list of fields, used to [sort search results](https://www.elastic.co/guide/en/elasticsearch/reference/7.4/search-request-sort.html), in order of importance.

An empty body matches all documents in the queried collection.

## Return

Returns a [SearchResults](/sdk/csharp/2/core-classes/search-results) instance.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/search.cs

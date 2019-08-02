---
code: true
type: page
title: DeleteByQueryAsync
description: Delete documents matching query
---

# DeleteByQueryAsync

Deletes documents matching the provided search query.

Kuzzle uses the [ElasticSearch Query DSL](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/query-dsl.html) syntax.

## Signature

```csharp
public async Task<JArray> DeleteByQueryAsync(
  string index, 
  string collection, 
  JObject query);

```

## Arguments

| Argument     | Type                                 | Description                             |
| ------------ | ------------------------------------ | --------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                              |
| `collection` | <pre>string</pre>        | Collection name                         |
| `query`      | <pre>JObject</pre>        | JObject representing the query to match |


## Return

A JArray containing the ids of deleted documents.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/delete-by-query.cs

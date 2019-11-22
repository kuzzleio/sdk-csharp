---
code: true
type: page
title: CountAsync
description: Counts documents matching the given query.
---

# CountAsync

Counts documents in a collection.

A query can be provided to alter the count result, otherwise returns the total number of documents in the collection.

Kuzzle uses the [ElasticSearch Query DSL](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/query-dsl.html) syntax.

## Arguments

```csharp
public async Task<int> CountAsync(
  string index, 
  string collection, 
  JObject query = null);

```

<br/>

| Argument     | Type                                 | Description                                 |
| ------------ | ------------------------------------ | ------------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                                  |
| `collection` | <pre>string</pre>        | Collection name                             |
| `query`      | <pre>JObject</pre>        | JObject representing the query to match |

## Return

Returns the number of matched documents.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/count.cs

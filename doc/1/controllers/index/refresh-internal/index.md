---
code: true
type: page
title: RefreshInternalAsync
description: Force refresh of Kuzzle internal index
---

# RefreshInternalAsync

When writing or deleting security and internal documents (users, roles, profiles, configuration, etc.) in Kuzzle, the update needs to be indexed before being reflected in the search index.

The `refreshInternal` action forces a [refresh](/sdk/csharp/1/controllers/index/refresh/), on the internal index, making the documents available to search immediately.

:::info
A refresh operation comes with some performance costs.

From the [Elasticsearch documentation](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/docs-refresh.html):
> "While a refresh is much lighter than a commit, it still has a performance cost. A manual refresh can be useful when writing tests, but don’t do a manual refresh every time you index a document in production; it will hurt your performance. Instead, your application needs to be aware of the near real-time nature of Elasticsearch and make allowances for it."

:::

## Arguments

```cs
Task RefreshInternalAsync();
```

## Usage

<<< ./snippets/refreshInternal.cs

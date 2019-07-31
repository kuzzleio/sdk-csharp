---
code: true
type: page
title: RefreshAsync
description: Force Elasticsearch search index update
---

# RefreshAsync

When writing or deleting documents in Kuzzle, the update needs to be indexed before being available in search results.

:::info
A refresh operation comes with some performance costs.

From the [Elasticsearch documentation](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/docs-refresh.html):
> "While a refresh is much lighter than a commit, it still has a performance cost. A manual refresh can be useful when writing tests, but don’t do a manual refresh every time you index a document in production; it will hurt your performance. Instead, your application needs to be aware of the near real-time nature of Elasticsearch and make allowances for it."

:::

## Arguments

```cs
Task RefreshAsync(string index);
```

| Arguments | Type                       | Description       |
| --------- | -------------------------- | ----------------- |
| `index`   | string                     | Index name        |

## Usage

<<< ./snippets/refresh.cs

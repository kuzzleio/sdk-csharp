---
code: true
type: page
title: GetAutoRefreshAsync
description: Returns the status of autorefresh flag.
---

# GetAutoRefreshAsync

The getAutoRefresh action returns the current autorefresh status for the index.

Each index has an autorefresh flag.  
When set to true, each write request triggers a [refresh](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/docs-refresh.html) action in Elasticsearch.  
Without a refresh after a write request, the documents may not be immediately available in search results.

:::info
A refresh operation comes with some performance costs.  
While forcing the autoRefresh can be convenient on a development or test environment,  
we recommend that you avoid using it in production or at least carefully monitor its implications before using it.
:::

## Arguments

```cs
Task<bool> GetAutoRefreshAsync(string index);
```

| Argument | Type              | Description |
|----------|-------------------|-------------|
| `index`  | <pre>string</pre> | Index name  |

## Return

Returns a `bool` that indicate the status of the **autoRefresh** flag.

## Usage

<<< ./snippets/getAutoRefresh.cs

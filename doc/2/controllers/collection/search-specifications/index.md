---
code: true
type: page
title: SearchSpecificationsAsync
description: Searches collection specifications.
---

# SearchSpecificationsAsync

Searches collection specifications.

There is a limit to how many items can be returned by a single search query.
That limit is by default set at 10000, and you can't get over it even with the from and size pagination options.

:::info
When processing a large number of items (i.e. more than 1000), it is advised to paginate the results using [SearchResult.next](/sdk/csharp/2/core-classes/search-result/next) rather than increasing the size parameter.
:::


## Arguments

```csharp
public async Task<DataObjects.SearchResults> SearchSpecificationsAsync(
        JObject filters,
        Options.SearchOptions options = null);
```

| Argument  | Type                             | Description                             |
|-----------|----------------------------------|-----------------------------------------|
| `filters` | <pre>JObject</pre>               | JObject representing the query to match |
| `options` | <pre>Options.SearchOptions</pre> | Query options                           |

### options

| Options  | Type                           | Description                                                                                                                                                                                                          |
|----------|--------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `From`   | <pre>int</pre><br/>(`0`)       | Offset of the first document to fetch                                                                                                                                                                                |
| `Size`   | <pre>int</pre><br/>(`10`)      | Maximum number of documents to retrieve per page                                                                                                                                                                     |
| `Scroll` | <pre>string</pre><br/>(`null`) | When set, gets a forward-only cursor having its TTL set to the given value (i.e. `30s`; cf. [Elasticsearch time limits](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/common-options.html#time-units)) |

## Return

Returns a [kuzzleio::SearchResult](/sdk/csharp/2/core-classes/search-result).

## Exceptions

Throws a `kuzzleio::KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/search-specifications.cs

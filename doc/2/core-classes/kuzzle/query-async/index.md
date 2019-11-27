---
code: true
type: page
title: QueryAsync
description: Base method to send API query to Kuzzle
---

# query

Base method used to send queries to Kuzzle, following the [API Documentation](/core/2/api).

:::warning
This is a low-level method, exposed to allow advanced SDK users to bypass high-level methods.
:::

## Arguments

```csharp
public ConfiguredTaskAwaitable<Response> QueryAsync(JObject query)
```

<br/>

| Argument  | Type              | Description            |
| --------- | ----------------- | ---------------------- |
| `query` | <pre>JObject</pre> | API request    |

### query

All properties necessary for the Kuzzle API can be added in the query object.
The following properties are the most common.

| Property     | Type              | Description                              |
| ------------ | ----------------- | ---------------------------------------- |
| `controller` | <pre>string</pre> | Controller name (mandatory)              |
| `action`     | <pre>string</pre> | Action name (mandatory)                  |
| `body`       | <pre>object</pre> | Query body for this action               |
| `index`      | <pre>string</pre> | Index name for this action               |
| `collection` | <pre>string</pre> | Collection name for this action          |
| `_id`        | <pre>string</pre> | id for this action                       |
| `volatile`   | <pre>object</pre> | Additional information to send to Kuzzle |

## Returns

Returns a [Response](/sdk/csharp/2/core-classes/response) object which represent a raw Kuzzle API response. See the [API Documentation](/core/2/api).

## Usage

<<< ./snippets/query.cs

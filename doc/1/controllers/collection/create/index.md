---
code: true
type: page
title: CreateAsync
description: Create a new collection
---

# CreateAsync

Creates a new [collection](/core/1/guides/essentials/store-access-data/) in the provided `index`.
You can also provide an optional data mapping that allow you to exploit the full capabilities of our
persistent data storage layer, [ElasticSearch](https://www.elastic.co/products/elasticsearch) (check here the [mapping capabilities of ElasticSearch](https://www.elastic.co/guide/en/elasticsearch/reference/5.4/mapping.html)).

This method will only update the mapping if the collection already exists.

## Signature

```csharp
public async Task CreateAsync(
        string index,
        string collection,
        JObject mappings = null);
```

## Arguments

| Arguments    | Type                           | Description                                      |
|--------------|--------------------------------|--------------------------------------------------|
| `index`      | <pre>string&</pre>             | Index name                                       |
| `collection` | <pre>string&</pre>             | Collection name                                  |
| `mapping`    | <pre>JObject</pre><br>(`null`) | JObject representing the collection data mapping |

### mapping

A JObject representing the collection data mapping.

The mapping must have a root field `properties` that contain the mapping definition:

```json
{
  "properties": {
    "field1": { "type": "text" },
    "field2": {
      "properties": {
        "nestedField": { "type": "keyword" }
      }
    }
  }
}
```

More informations about database mappings [here](/core/1/guides/essentials/database-mappings).

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/create.cs

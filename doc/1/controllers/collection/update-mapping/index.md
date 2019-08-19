---
code: true
type: page
title: UpdateMappingAsync
description: Update the collection mapping.
---

# updateMapping

Update the collection mapping.
Mapping allow you to exploit the full capabilities of our
persistent data storage layer, [ElasticSearch](https://www.elastic.co/products/elasticsearch) (check here the [mapping capabilities of ElasticSearch](https://www.elastic.co/guide/en/elasticsearch/reference/5.6/mapping.html)).


## Arguments

```csharp
public async Task UpdateMappingAsync(
        string index,
        string collection,
        JObject mappings);
```

| Argument     | Type               | Description                                      |
|--------------|--------------------|--------------------------------------------------|
| `index`      | <pre>string</pre>  | Index name                                       |
| `collection` | <pre>string</pre>  | Collection name                                  |
| `mappings`   | <pre>JObject</pre> | JObject representing the collection data mapping |

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

### options

Additional query options

| Property   | Type                         | Description                                                                  |
|------------|------------------------------|------------------------------------------------------------------------------|
| `queuable` | <pre>bool</pre><br/>(`true`) | If true, queues the request during downtime, until connected to Kuzzle again |

## Usage

<<< ./snippets/update-mapping.cs

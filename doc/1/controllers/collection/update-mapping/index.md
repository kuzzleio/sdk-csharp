---
code: true
type: page
title: UpdateMappingAsync
description: Update the collection mapping.
---

# updateMapping

Updates the collection mappings.

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

- `dynamic`: [dynamic mapping policy](/core/1/guides/essentials/database-mappings#dynamic-mapping-policy) for new fields. Allowed values: `true` (default), `false`, `strict`
- `_meta`: [collection additional metadata](core/1/guides/essentials/database-mappings#collection-metadata) stored next to the collection
- `properties`: object describing the data mapping to associate to the new collection, using [Elasticsearch types definitions format](object describing the data mapping to associate to the new collection, using)

```json
{
  "dynamic": "[true|false|strict]",
  "_meta": {
    "field": "value"
  },
  "properties": {
    "field1": {
      "type": "integer"
    },
    "field2": {
      "type": "keyword"
    },
    "field3": {
      "type":   "date",
      "format": "yyyy-MM-dd HH:mm:ss||yyyy-MM-dd||epoch_millis"
    }
  }
}
```

More information about database mappings [here](/core/1/guides/essentials/database-mappings).

## Usage

<<< ./snippets/update-mapping.cs

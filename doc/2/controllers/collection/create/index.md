---
code: true
type: page
title: CreateAsync
description: Create a new collection.
---

# CreateAsync

Creates a new [collection](/core/2/guides/essentials/store-access-data) in the provided `index`.
You can also provide an optional body with a [collection mapping](/core/2/guides/essentials/database-mappings) allowing you to exploit the full capabilities of our persistent data storage layer.

This method will only update the mapping if the collection already exists.


## Arguments

```csharp
public async Task CreateAsync(
        string index,
        string collection,
        JObject mappings = null);
```

| Argument     | Type                           | Description                                      |
|--------------|--------------------------------|--------------------------------------------------|
| `index`      | <pre>string</pre>             | Index name                                       |
| `collection` | <pre>string</pre>             | Collection name                                  |
| `mapping`    | <pre>JObject</pre><br>(`null`) | JObject representing the collection data mapping |

### mapping

A JObject representing the collection data mapping.

The mapping must have a root field `properties` containing the mapping definition:

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

More information about database mappings [here](/core/2/guides/essentials/database-mappings).

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/create.cs

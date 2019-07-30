---
code: true
type: page
title: GetMappingAsync
description: Return collection mapping
---

# GetMappingAsync

Returns the mapping for the given collection.

## Signature

```csharp
public async Task<JObject> GetMappingAsync(
        string index,
        string collection);
```

## Arguments

| Arguments    | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Return

A JObject representing the collection data mapping.

## Usage

<<< ./snippets/get-mapping.cs

---
code: true
type: page
title: GetSpecificationsAsync
description: Returns the validation specifications.
---

# GetSpecificationsAsync

Returns the validation specifications associated to the collection.


## Arguments

```csharp
public async Task<JObject> GetSpecificationsAsync(
    string index,
    string collection);
```

| Argument     | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Return

A JObject representing the validation specifications.

## Usage

<<< ./snippets/get-specifications.cs

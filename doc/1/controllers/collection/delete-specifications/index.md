---
code: true
type: page
title: DeleteSpecificationsAsync
description: Delete validation specifications for a collection
---

# DeleteSpecificationsAsync

Delete the validation specifications associated with the collection.

## Signature

```csharp
public async Task DeleteSpecificationsAsync(
    string index,
    string collection);
```

## Arguments

| Arguments    | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Usage

<<< ./snippets/delete-specifications.cs

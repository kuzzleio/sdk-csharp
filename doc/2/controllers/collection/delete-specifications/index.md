---
code: true
type: page
title: DeleteSpecificationsAsync
description: Delete validation specifications for a collection.
---

# DeleteSpecificationsAsync

Deletes the validation specifications associated with the collection.


## Arguments

```csharp
public async Task DeleteSpecificationsAsync(
    string index,
    string collection);
```

| Argument     | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Usage

<<< ./snippets/delete-specifications.cs

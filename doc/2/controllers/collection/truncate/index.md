---
code: true
type: page
title: TruncateAsync
description: Remove all documents from collection.
---

# TruncateAsync

Removes all documents from a collection while keeping the associated mappings.  


## Arguments

```csharp
public async Task TruncateAsync(
        string index,
        string collection);
```

| Argument     | Type              | Description     |
|--------------|-------------------|-----------------|
| `index`      | <pre>string</pre> | Index name      |
| `collection` | <pre>string</pre> | Collection name |

## Usage

<<< ./snippets/truncate.cs

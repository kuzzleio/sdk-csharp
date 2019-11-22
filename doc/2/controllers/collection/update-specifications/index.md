---
code: true
type: page
title: UpdateSpecificationsAsync
description: Update the validation specifications.
---

# updateSpecifications

Creates or updates the validation specifications for one or more index/collection pairs.

When the validation specifications are not correctly formatted, a detailed error message is returned to help with debugging.


## Arguments

```csharp
public async Task<JObject> UpdateSpecificationsAsync(
        string index,
        string collection,
        JObject specifications);
```

| Argument         | Type               | Description                  |
|------------------|--------------------|------------------------------|
| `index`          | <pre>string</pre>  | Index name                   |
| `collection`     | <pre>string</pre>  | Collection name              |
| `specifications` | <pre>JObject</pre> | Specifications in JSON format |

### specifications

A JObject representing the specifications.

The JSON must follow the [Specifications structure](/core/2/guides/cookbooks/datavalidation/schema):

```js
{
  "strict": "true",
  "fields": {
    "licence": {
      "mandatory": true,
      "type": "string"
    }
    // ... specification for each field
  }
}
```

## Return

A JObject representing the specifications.

## Usage

<<< ./snippets/update-specifications.cs

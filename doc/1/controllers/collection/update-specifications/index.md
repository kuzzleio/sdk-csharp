---
code: true
type: page
title: UpdateSpecificationsAsync
description: Update the validation specifications.
---

# updateSpecifications

The updateSpecifications method allows you to create or update the validation specifications for one or more index/collection pairs.

When the validation specification is not formatted correctly, a detailed error message is returned to help you to debug.


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
| `specifications` | <pre>JObject</pre> | Specification in JSON format |

### specifications

A JObject representing the specifications.

The JSON must follow the [Specification Structure](/core/1/guides/cookbooks/datavalidation/schema):

```json
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

A JObject representing the specifications

## Usage

<<< ./snippets/update-specifications.cs

---
code: true
type: page
title: ValidateSpecificationsAsync
description: Validate specifications format.
---

# ValidateSpecificationsAsync

The validateSpecifications method checks if validation specifications are well-formed. It does not store nor modify the existing specifications.

If the validation specifications are not formatted correctly, a detailed error message is returned to help with debugging.


## Arguments

```csharp
public async Task<bool> ValidateSpecificationsAsync(
        string index,
        string collection,
        JObject specifications);
```

| Argument         | Type               | Description                                           |
|------------------|--------------------|-------------------------------------------------------|
| `index`          | <pre>string</pre>  | Index name                   |
| `collection`     | <pre>string</pre>  | Collection name              |
| `specifications` | <pre>JObject</pre> | JObject representating the specifications to validate |

### specifications

A JObject representing the specifications to validate.

The JObject data must follow the [Specifications Structure](/core/2/guides/advanced/data-validation):

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

A boolean telling whether the provided specifications are valid.

## Usage

<<< ./snippets/validate-specifications.cs

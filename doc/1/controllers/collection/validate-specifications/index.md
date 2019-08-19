---
code: true
type: page
title: ValidateSpecificationsAsync
description: Validate specifications format.
---

# ValidateSpecificationsAsync

The validateSpecifications method checks if a validation specification is well formatted. It does not store nor modify the existing specification.

When the validation specification is not formatted correctly, a detailed error message is returned to help you to debug.


## Arguments

```csharp
public async Task<bool> ValidateSpecificationsAsync(JObject specifications);
```

| Argument         | Type               | Description                                           |
|------------------|--------------------|-------------------------------------------------------|
| `specifications` | <pre>JObject</pre> | JObject representating the specifications to validate |

### specifications

A JObject representing the specifications the specifications.

The JObject must follow the [Specification Structure](/core/1/guides/cookbooks/datavalidation):

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

A boolean telling whether the provided specifications are valid.

## Usage

<<< ./snippets/validate-specifications.cs

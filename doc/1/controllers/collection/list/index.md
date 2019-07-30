---
code: true
type: page
title: ListAsync
description: Returns the collection list of an index
---

# ListAsync

Returns the complete list of realtime and stored collections in requested index sorted by name in alphanumerical order.
The `from` and `size` arguments allow pagination. They are returned in the response if provided.

## Signature

```csharp
public async Task<JObject> ListAsync(
      string index,
      int? from = null,
      int? size = null,
      string type = null
    );
```

## Arguments

| Arguments | Type              | Description |
|-----------|-------------------|-------------|
| `index`   | <pre>string</pre> | Index name  |

### options

Additional query options

| Property | Type<br/>(default)            | Description                                                                                         |
|----------|-------------------------------|-----------------------------------------------------------------------------------------------------|
| `from`   | <pre>int?</pre><br>(`null`)   | Offset of the first result                                                                          |
| `size`   | <pre>int?</pre><br>(`null`)   | Maximum number of returned results                                                                  |
| `type`   | <pre>string</pre><br>(`null`) | filters the returned collections. Allowed values: `all`, `stored` and `realtime` (default : `all`). |

## Return

A JObject representing the following object:

| Property      | Type              | Description                                                        |
|---------------|-------------------|--------------------------------------------------------------------|
| `type`        | <pre>string</pre> | Types of returned collections <br/>(`all`, `realtime` or `stored`) |
| `collections` | <pre>JArray</pre> | List of collections                                                |

Each object in the `collections` array contains the following properties:

| Property | Type              | Description                              |
| -------- | ----------------- | ---------------------------------------- |
| `name`   | <pre>string</pre> | Collection name                          |
| `type`   | <pre>string</pre> | Collection type (`realtime` or `stored`) |

## Usage

<<< ./snippets/list.cs

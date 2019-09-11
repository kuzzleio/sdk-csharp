---
code: true
type: page
title: ListAsync
description: Returns the collection list of an index.
---

# ListAsync

Returns the list of collections associated to a provided index.

The returned list is sorted in alphanumerical order.
The `from` and `size` arguments allow pagination. They are returned in the response if provided.


## Arguments

```csharp
public async Task<JObject> ListAsync(
      string index,
      int? from = null,
      int? size = null,
      TypeFilter type = TypeFilter.All
    );
```

| Argument | Type                             | Description                                                                                         |
|----------|----------------------------------|-----------------------------------------------------------------------------------------------------|
| `index`  | <pre>string</pre>                | Index name                                                                                          |
| `from`   | <pre>int?</pre><br>(`null`)      | Offset of the first result                                                                          |
| `size`   | <pre>int?</pre><br>(`null`)      | Maximum number of returned results                                                                  |
| `type`   | <pre>TypeFilter</pre><br>(`All`) | Filters the returned collections. Allowed values: `All`, `Stored` and `Realtime` (default : `All`). |

## Return

A JObject representing the following object:

| Property      | Type              | Description                                                        |
|---------------|-------------------|--------------------------------------------------------------------|
| `type`        | <pre>string</pre> | Types of returned collections <br/>(`all`, `realtime` or `stored`) |
| `collections` | <pre>JArray</pre> | List of collections                                                |

Each object in the `collections` array contains the following properties:

| Property | Type              | Description                              |
|----------|-------------------|------------------------------------------|
| `name`   | <pre>string</pre> | Collection name                          |
| `type`   | <pre>string</pre> | Collection type (`realtime` or `stored`) |

## Usage

<<< ./snippets/list.cs

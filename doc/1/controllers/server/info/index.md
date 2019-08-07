---
code: true
type: page
title: InfoAsync
description: Returns information about Kuzzle server.
---

# InfoAsync

Returns information about Kuzzle: available API (base + extended), plugins, external services (Redis, Elasticsearch, ...), servers, etc.

## Arguments

```csharp
async Task<JObject> InfoAsync()
```

## Return

Returns server information as a `JObject`.

## Usage

<<< ./snippets/info.cs

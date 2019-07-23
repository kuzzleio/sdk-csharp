---
code: true
type: page
title: InfoAsync
description: Returns information about Kuzzle server.
---

# InfoAsync

Returns information about Kuzzle: available API (base + extended), plugins, external services (Redis, Elasticsearch, ...), servers, etc.

## Signature

```csharp
async Task<JObject> InfoAsync()
```

## Return

Return server information as a `JObject`.

## Usage

<<< ./snippets/info.cs

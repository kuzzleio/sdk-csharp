---
code: true
type: page
title: info
description: Returns information about Kuzzle server.
---

# info

Returns information about Kuzzle: available API (base + extended), plugins, external services (Redis, Elasticsearch, ...), servers, etc.

## Signature

```csharp
async Task<JObject> InfoAsync()
```

## Return

Return server information as a `JObject`.

## Usage

<<< ./snippets/info.cs

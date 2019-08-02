---
code: true
type: page
title: GetConfigAsync
description: Returns the current Kuzzle configuration.
---

# GetConfigAsync

Returns the current Kuzzle configuration.

:::warning
This route should only be accessible to administrators, as it might return sensitive information about the backend.
:::

## Arguments

```csharp
async Task<JObject> GetConfigAsync()
```

## Return

Return server configuration as a `JObject`.

## Usage

<<< ./snippets/getConfig.cs

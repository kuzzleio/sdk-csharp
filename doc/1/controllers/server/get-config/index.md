---
code: true
type: page
title: getConfig
description: Returns the current Kuzzle configuration.
---

# getConfig

Returns the current Kuzzle configuration.

:::warning
This route should only be accessible to administrators, as it might return sensitive information about the backend.
:::

## Signature

```csharp
async Task<JObject> GetConfigAsync()
```

## Return

Return server configuration as a `JObject`.

## Usage

<<< ./snippets/getConfig.cs

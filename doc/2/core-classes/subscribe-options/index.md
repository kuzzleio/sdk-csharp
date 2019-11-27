---
code: true
type: page
title: SubscribeOptions
description: SubscribeOptions class documentation
order: 110
---

# SubscribeOptions

This class represents the options usable with the [Realtime.subscribe](/sdk/csharp/2/controllers/realtime/subscribe) method.  

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk.API.Options;
```

## Properties

| Property | Type<br/>(default) | Description      | writable |
|----------|--------------------|------------------| ------- |
| `Scope`           | <pre>string</pre><br/>(`all`)   | Subscribes to document entering or leaving the scope<br/>Possible values: `all`, `in`, `out`, `none`| yes |
| `Users`           | <pre>string</pre><br/>(`none`)  | Subscribes to users entering or leaving the room<br/>Possible values: `all`, `in`, `out`, `none`| yes |
| `SubscribeToSelf` | <pre>bool</pre><br/>(`true`)    | Subscribes to notifications fired by our own queries | yes |
| `Volatile`        | <pre>JObject</pre><br/>(`null`) | JObject representing subscription information, used in [user join/leave notifications](/core/2/api/essentials/volatile-data)  |yes |

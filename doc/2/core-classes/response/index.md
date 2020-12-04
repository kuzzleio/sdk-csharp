---
code: true
type: page
title: Response
description: Response class documentation
order: 100
---

# Response

This class represents a raw [Kuzzle API response](/core/2/api/payloads/response/).  

## Namespace

You must include the following namespace: 

```csharp
using KuzzleSdk.API;
```

## Properties

| Property | Type | Description | writable |
|--- |--- |--- | --- |
| `Action` | <pre>string</pre> | Executed Kuzzle API controller's action | no |
| `Controller` | <pre>string</pre> | Executed Kuzzle API controller | no |
| `Collection` | <pre>string</pre> | Impacted data collection | no |
| `Error` | <pre>ErrorResponse</pre> | Error object (null if the request finished successfully) | no |
| `Index` | <pre>string</pre> | Impacted data index | no |
| `RequestId` | <pre>string</pre> | Request unique identifier | no |
| `Result` | <pre>JToken</pre> | Response payload (depends on the executed API action) | no |
| `Status` | <pre>int</pre> | Response status, following HTTP status codes | no |
| `Volatile` | <pre>JObject</pre> | Volatile data | no |

**Properties specific to [real-time notifications](/sdk/csharp/2/essentials/realtime-notifications/)**

| Property | Type | Description | writable |
|--- |--- |--- | --- |
| `Protocol` | <pre>string</pre> | Network protocol at the origin of the real-time notification | no |
| `Room` | <pre>string</pre> | Room unique identifier | no |
| `Scope` | <pre>string</pre> | Document scope ("in" or "out") | no |
| `State` | <pre>string</pre> | Document state | no |
| `Timestamp` | <pre>long</pre> | Notification timestamp (UTC) | no |
| `Type` | <pre>string</pre> | Notification type | no |

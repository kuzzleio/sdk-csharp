---
code: true
type: page
title: subscribe
description: Subscribe to real-time notifications
---

# subscribe

Subscribes by providing a set of filters: messages, document changes and, optionally, user events matching the provided filters will generate [real-time notifications](/core/1/api/essentials/notifications/), sent to you in real-time by Kuzzle.

## Signature

```csharp
public async Task<string> SubscribeAsync(
        string index, string collection, JObject filters,
        NotificationHandler handler, SubscribeOptions options = null);
```

## Arguments

| Arguments    | Type                                    | Description                                                                                                     |
|--------------|-----------------------------------------|-----------------------------------------------------------------------------------------------------------------|
| `index`      | <pre>string</pre>                       | Index name                                                                                                      |
| `collection` | <pre>string</pre>                       | Collection name                                                                                                 |
| `filters`    | <pre>JObject</pre>                      | JObject representing a set of filters following [Koncorde syntax](/core/1/guides/cookbooks/realtime-api/terms/) |
| `listener`   | <pre>NotificationHandler</pre>          | Listener function to handle notifications                                                                       |
| `options`    | <pre>SubscribeOptions</pre><br>(`null`) | Subscription options                                                                                            |

### listener

Listener function that will be called each time a new notifications is received.
The listener will receive a [const kuzzleio::notification_result\*](/sdk/csharp/1/essentials/realtime-notifications) as only argument.

### options

Additional subscription options.

| Property          | Type<br/>(default)              | Description                                                                                                                   |
|-------------------|---------------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| `scope`           | <pre>string</pre><br/>(`all`)   | Subscribe to document entering or leaving the scope<br/>Possible values: `all`, `in`, `out`, `none`                           |
| `users`           | <pre>string</pre><br/>(`none`)  | Subscribe to users entering or leaving the room<br/>Possible values: `all`, `in`, `out`, `none`                               |
| `subscribeToSelf` | <pre>bool</pre><br/>(`true`)    | Subscribe to notifications fired by our own queries                                                                           |
| `volatile`        | <pre>JObject</pre><br/>(`null`) | JObject representing subscription information, used in [user join/leave notifications](/core/1/api/essentials/volatile-data/) |

## Return

The room ID.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

_Simple subscription to document notifications_

<<< ./snippets/document-notifications.cs

_Subscription to document notifications with scope option_

<<< ./snippets/document-notifications-leave-scope.cs

_Subscription to message notifications_

<<< ./snippets/message-notifications.cs

_Subscription to user notifications_

<<< ./snippets/user-notifications.cs

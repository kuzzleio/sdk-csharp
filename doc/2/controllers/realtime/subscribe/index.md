---
code: true
type: page
title: SubscribeAsync
description: Subscribes to real-time notifications.
---

# SubscribeAsync

Subscribes by providing a set of filters: messages, document changes and, optionally, user events matching the provided filters will generate [real-time notifications](/core/2/api/essentials/notifications), sent to you in real-time by Kuzzle.

## Arguments

```csharp
public async Task<string> SubscribeAsync(
        string index,
        string collection,
        JObject filters,
        NotificationHandler handler,
        SubscribeOptions options = null);
```

| Argument     | Type                                    | Description                                                                                                    |
|--------------|-----------------------------------------|----------------------------------------------------------------------------------------------------------------|
| `index`      | <pre>string</pre>                       | Index name                                                                                                     |
| `collection` | <pre>string</pre>                       | Collection name                                                                                                |
| `filters`    | <pre>JObject</pre>                      | JObject representing a set of filters following [Koncorde syntax](/core/2/guides/cookbooks/realtime-api/terms) |
| `handler`   | <pre>NotificationHandler</pre>          | Handler function to handle notifications                                                                      |
| `options`    | <pre>SubscribeOptions</pre><br>(`null`) | Subscription options                                                                                           |

### handled

Handler function that will be called each time a new notification is received.
The hanlder will receive a [Response](/sdk/csharp/2/essentials/realtime-notifications) as only argument.

### options

A [SubscribeOptions](/sdk/csharp/2/core-classes/subscribe-options) object.

## Return

The room ID.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).

## Usage

_Simple subscription to document notifications_

<<< ./snippets/document-notifications.cs

_Subscription to document notifications with scope option_

<<< ./snippets/document-notifications-leave-scope.cs

_Subscription to message notifications_

<<< ./snippets/message-notifications.cs

_Subscription to user notifications_

<<< ./snippets/user-notifications.cs

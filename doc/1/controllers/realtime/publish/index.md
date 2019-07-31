---
code: true
type: page
title: PublishAsync
description: Publish a real-time message
---

# PublishAsync

Sends a real-time message to Kuzzle. The message will be broadcasted to all clients with subscriptions matching the index, the collection and the message content.

The index and collection are indicative and serve only to distinguish the rooms. They are not required to exist in the database.

**Note:** real-time messages are not persisted in the database.

## Arguments

```csharp
public async Task PublishAsync(string index, string collection, JObject message);
```

| Argument     | Type               | Description                         |
|--------------|--------------------|-------------------------------------|
| `index`      | <pre>string</pre>  | Index name                          |
| `collection` | <pre>string</pre>  | Collection name                     |
| `message`    | <pre>JObject</pre> | JObject representing a JSON payload |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/publish.cs

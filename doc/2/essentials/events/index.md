---
code: false
type: page
title: Events
description: SDK events system
order: 200
---

# Events

An event system allows to be notified when the SDK status changes. These events are issued by the [Kuzzle](/sdk/csharp/2/core-classes/kuzzle) SDK object.

The API for interacting with events is described by our [KuzzleEventHandler](/sdk/csharp/2/core-classes/kuzzle-event-handler) class documentation.

**Note:** listeners are called in the order of their insertion.

# Emitted Events

## QueueRecovered

Occurs when the offline queue of query has been successfuly recovered.

### Handler Arguments

```csharp
Handler();
```

```csharp
kuzzle.EventHandler.QueueRecovered += () => {
  Console.WriteLine("Offline queue has successfully recovered.");
}
```

## Reconnected

Occurs when the successfuly reconnects to the network.

### Handler Arguments

```csharp
Handler();
```

```csharp
kuzzle.EventHandler.Reconnected += () => {
  Console.WriteLine("SDK successfully reconnects to Kuzzle");
}
```

## TokenExpired

Occurs when the SDK sends a request and the current token has expired.

### Handler Arguments

```csharp
Handler();
```

```csharp
kuzzle.EventHandler.TokenExpired += () => {
  Console.WriteLine("The current authentication token has expired");
}
```

## UnhandledResponse

Occurs when an unhandled response is received.

### Handler Arguments

```csharp
Handler(KuzzleEventHandler sender, Response response);
```

| Name | Type               | Description                       |
| ---- | ------------------ | --------------------------------- |
| `sender` | <pre>KuzzleEventHandler</pre> | KuzzleEventHandler instance |
| `response` | <pre>Response</pre>  | Unhandled Kuzzle API response     |

```csharp
kuzzle.EventHandler.UnhandledResponse += (object sender, Response response) => {
  Console.WriteLine($"Unhandled response {response.ToString()}");
}
```

## UserLoggedIn

Occurs when a user has logged in.

### Handler Arguments

```csharp
Handler(KuzzleEventHandler sender, UserLoggedInEvent user);
```

| Name | Type               | Description                       |
| ---- | ------------------ | --------------------------------- |
| `sender` | <pre>KuzzleEventHandler</pre> | KuzzleEventHandler instance |
| `user` | <pre>UserLoggedInEvent</pre>  | Contain the `kuid` of the logged in user     |

```csharp
kuzzle.EventHandler.UserLoggedIn += (object sender, UserLoggedInEvent user) => {
  Console.WriteLine($"User {user.Kuid} has logged in");
}
```

## UserLoggedOut

Occurs when a user has logged out.

### Handler Arguments

```csharp
Handler();
```

```csharp
kuzzle.EventHandler.UserLoggedOut += () => {
  Console.WriteLine("User has logged out");
}
```

---
code: true
type: page
title: Properties
description: Properties for Offline Tools
---

# OfflineManager class properties

## MaxQueueSize

The maximum amount of elements that the offline queue can contain.

### Definition

```csharp
public int MaxQueueSize { get; set; }
```

## MaxRequestDelay

The maximum delay between two replayed requests.

### Definition

```csharp
public int MaxRequestDelay { get; set; }
```

## QueueFilter

The custom queue filter applied when a queue is being replayed.

### Definition

```csharp
public Func<JObject, bool> QueueFilter { get; set; }
```

## RefreshedTokenDuration

The minimum duration of a token after being refreshed.

### Definition

```csharp
public int RefreshedTokenDuration { get; set; }
```
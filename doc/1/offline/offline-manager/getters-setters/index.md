---
code: true
type: page
title: Getters and Setters
desription: Getters and Setters for Offline Tools
---

# OfflineManager class properties

## MaxQueueSize

Gets or sets the maximum amount of elements that the offline queue can contain.

### Definition

```csharp
public int MaxQueueSize { get; set; }
```

## MaxRequestDelay

Gets or sets the maximum delay between two replayed requests.

### Definition

```csharp
public int MaxRequestDelay { get; set; }
```

## QueueFilter

Gets or sets the custom queue filter applied when a queue is being replayed.

### Definition

```csharp
public Func<JObject, bool> QueueFilter { get; set; }
```

## RefreshedTokenDuration

Gets or sets the minimum duration of a token after being refreshed.

### Definition

```csharp
public int RefreshedTokenDuration { get; set; }
```

## Usage

<<< ./snippets/setters.cs


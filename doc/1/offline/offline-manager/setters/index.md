---
code: true
type: page
title: Setters
order: 100
desription: Setters for Offline
---

# OfflineManager class setters



## MaxQueueSize

Return the maximum amount of elements that the offline queue can contains.

### Signature

```csharp
public int MaxQueueSize {
    get { return maxQueueSize; }
    set { maxQueueSize = value < 0 ? -1 : value; }
}
```

## MinTokenDuration

Return the minimum duration of a Token after refresh.

### Signature

```csharp
public int MinTokenDuration {
    get { return minTokenDuration; }
    set { minTokenDuration = value < 0 ? -1 : value; }
}
```

## MaxRequestDelay

Return the maximum delay between two requests to be replayed

### Signature

```csharp
public int MaxRequestDelay {
    get { return maxRequestDelay; }
    set { maxRequestDelay = value < 0 ? 0 : value; }
}
```

## QueueFilter

Return the queue filter applied when a queue is being replayed.

### Signature

```csharp
public Func<JObject, bool> QueueFilter { get; set; }
```

## Usage

<<< ./snippets/setters.cs


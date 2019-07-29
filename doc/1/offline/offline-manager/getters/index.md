---
code: true
type: page
title: Getters
order: 100
desription: Getters for Offline
---

# OfflineManager class getters



## MaxQueueSize

Return the maximum amount of elements that the offline queue can contains.

### Arguments

```csharp
public int MaxQueueSize {
    get { return maxQueueSize; }
    set { maxQueueSize = value < 0 ? -1 : value; }
}
```

## MinTokenDuration

Return the minimum duration of a Token after refresh.

### Arguments

```csharp
public int MinTokenDuration {
    get { return minTokenDuration; }
    set { minTokenDuration = value < 0 ? -1 : value; }
}
```

## MaxRequestDelay

Return the maximum delay between two requests to be replayed

### Arguments

```csharp
public int MaxRequestDelay {
    get { return maxRequestDelay; }
    set { maxRequestDelay = value < 0 ? 0 : value; }
}
```

## QueueFilter

Return the queue filter applied when a queue is being replayed.

### Arguments

```csharp
public Func<JObject, bool> QueueFilter { get; set; }
```

## Usage

<<< ./snippets/getters.cs


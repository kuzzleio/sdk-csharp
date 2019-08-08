---
code: false
type: page
title: Error Handling
description: How to handle errors with the SDK
order: 100
---

# Error handling

All SDK methods can throw exception inheriting from [KuzzleException](/sdk/csharp/1/exceptions/kuzzle-exception) in case of failure.

The following exceptions are available:
  - [ApiErrorException](/sdk/csharp/1/exceptions/api-error-exception)
  - [ConnectionLostException](/sdk/csharp/1/exceptions/connection-lost-exception)
  - [InternalException](/sdk/csharp/1/exceptions/internal-exception)
  - [NotConnectionException](/sdk/csharp/1/exceptions/not-connected-exception)

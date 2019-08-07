---
code: false
type: page
title: Error Handling
description: How to handle errors with the SDK
order: 100
---

# Error handling

All SDK methods can throw subclasses of [KuzzleException](/sdk/csharp/1/exceptions/kuzzle-exception) in case of failure.

[KuzzleException](/sdk/csharp/1/exceptions/kuzzle-exception) inherits from the standard `System.Exception` class and add the following properties to it:

| Property | Type              | Description                                                                                |
| -------- | ----------------- | ------------------------------------------------------------------------------------------ |
| `Status` | <pre>int</pre>    | Status following [HTTP Standards](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) |

You can find a detailed list of possible errors messages and statuses in the [documentation API](/core/1/api/essentials/errors).

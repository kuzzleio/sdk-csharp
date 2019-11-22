---
code: true
type: page
title: ApiErrorException
description: ApiErrorException
order: 10
---

# ApiErrorException

Inherits from the [KuzzleException](/sdk/csharp/2/exceptions/kuzzle-exception) class.

The `ApiErrorException` exception is used to reject API requests upon receiving an [error response from Kuzzle's API](/core/2/api/essentials/errors/).

## Properties

These additional properties are available in addition to the [KuzzleException](/sdk/csharp/2/exceptions/kuzzle-exception) properties.

| Property name        | Type     | Description          |
| -------------------- | -------- | --------------------------------------- |
| `Stack`             | <pre>string</pre> | Error stacktrace (development only)      |

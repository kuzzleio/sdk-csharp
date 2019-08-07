---
code: false
type: page
title: ApiErrorException
description: ApiErrorException
order: 0
---

# ApiErrorException

Inherits from the [KuzzleException](/sdk/csharp/1/exceptions/kuzzle-exception) class.

The `ApiErrorException` class represents an [error response from Kuzzle API](/core/1/api/essentials/errors/).

## Properties

Theses additional properties are available in addition to the [KuzzleException](/sdk/csharp/1/exceptions/kuzzle-exception) properties.

| Property name        | Type     | Description          |
| -------------------- | -------- | --------------------------------------- |
| `Stack`             | <pre>string</pre> | Error stacktrace (development only)      |

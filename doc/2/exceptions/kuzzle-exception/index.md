---
code: true
type: page
title: KuzzleException
description: KuzzleException
order: 0
---

# KuzzleException

Inherits from the standard `System.Exception` class.

All other SDK exceptions inherits from this base class.

## Properties

These additional properties are available in addition to the [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception) properties.

| Property name        | Type     | Description          |
| -------------------- | -------- | --------------------------------------- |
| `Status`             | <pre>int</pre> | Error status code      |

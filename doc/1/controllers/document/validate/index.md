---
code: true
type: page
title: ValidateAsync
description: Validate a document
---

# ValidateAsync

Validates data against existing validation rules.

Documents are always valid if no validation rules are defined on the provided index and collection.

This request does not store the document.

## Signature

```csharp
public async Task<bool> ValidateAsync(
  string index, 
  string collection, 
  JObject content);

```

## Arguments

| Argument     | Type                                 | Description                           |
| ------------ | ------------------------------------ | ------------------------------------- |
| `index`      | <pre>string</pre>        | Index name                            |
| `collection` | <pre>string</pre>        | Collection name                       |
| `content`   | <pre>JObject</pre>        | JObject representing the document |

## Return

A bool set to true if the document is valid and false otherwise.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/validate.cs

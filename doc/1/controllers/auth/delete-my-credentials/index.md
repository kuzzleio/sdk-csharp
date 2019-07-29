---
code: true
type: page
title: deleteMyCredentials
description: Delete the current user's credentials for the specified strategy
---

# deleteMyCredentials

Delete the current user's credentials for the specified strategy. If the credentials that generated the current JWT are removed, the user will remain logged in until he logs out or his session expires, after that they will no longer be able to log in with the deleted credentials.

## Signature

```csharp
public async Task DeleteMyCredentialsAsync(string strategy);
```

## Arguments

| Arguments  | Type              | Description     |
|------------|-------------------|-----------------|
| `strategy` | <pre>string</pre> | Strategy to use |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/1/essentials/error-handling).

## Usage

<<< ./snippets/delete-my-credentials.cs

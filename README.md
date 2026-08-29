[![](https://img.shields.io/nuget/v/Soenneker.Extensions.List.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.List/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.list/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.list/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.List.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.List/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.list/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.list/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.List
A collection of helpful list extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.List
```

## Quick start

```csharp
using Soenneker.Extensions.List;

// Given an existing List<T>? named list:
list.Replace(match, newItem);
```

## Common operations

- `Replace()` - Replaces the first element in the list that matches the specified predicate with a new value.
- `Remove()` - Removes the first element in the list that matches the specified predicate.
- `Shuffle()` - Randomly reorders the elements of the list in place using a fast, non-cryptographic RNG.
- `SecureShuffle()` - Randomly reorders the elements of the list in place using a cryptographically secure RNG.
- `GetRandom()` - Returns a random element from the list, or the default value if the list is null or empty.
- `ToHashSet()` - Returns a new `HashSet<T>` containing the list items, or an empty set when the list is null or empty.
- `RemoveAll()` - Removes all elements from the list that match the specified predicate and state object. Returns the number of elements removed.

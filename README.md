[![](https://img.shields.io/nuget/v/Soenneker.Extensions.List.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.List/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.list/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.list/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.List.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.List/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.list/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.list/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.List
In-place replacement, removal, shuffling, random selection, and set conversion for `List<T>` and `IList<T>`.

## Installation

```bash
dotnet add package Soenneker.Extensions.List
```

## Replace or remove the first match

```csharp
using Soenneker.Extensions.List;

var jobs = new List<Job> {queued, running, completed};

jobs.Replace(job => job.Id == updated.Id, updated);
jobs.Remove(job => job.Id == obsoleteId);
```

`Replace()` changes only the first match. `Remove()` removes only the first match and preserves the order of every remaining item. A null or empty list is a no-op. A null predicate throws only when the list contains items.

Use the state-aware `RemoveAll()` when every match should be removed without a closure allocation:

```csharp
int removed = jobs.RemoveAll(
    static (job, state) => job.Status == (JobStatus)state!,
    JobStatus.Completed);
```

`RemoveAll()` compacts survivors in place, preserves their order, and returns the number removed. The list and predicate must not be mutated recursively while any of these operations are evaluating the predicate.

## Shuffle in place

```csharp
items.Shuffle();       // fast, non-cryptographic
items.SecureShuffle(); // cryptographic random source
```

Both methods use an unbiased Fisher-Yates shuffle and modify the supplied writable `IList<T>`. Null lists and lists with fewer than two items are unchanged. `Shuffle()` is appropriate for presentation order, retry spreading, and games that do not involve security. Use `SecureShuffle()` when the permutation must not be predictable, such as drawing prizes or security-sensitive assignments.

Neither method returns a copy. Create one first when the original order must be retained:

```csharp
var shuffled = new List<Card>(deck);
shuffled.Shuffle();
```

## Select one item

```csharp
Server? server = servers.GetRandom();
```

`GetRandom()` uses the same fast, non-cryptographic source as `Shuffle()`. It returns `default(T)` for a null or empty list, so the result cannot distinguish an empty list from a list whose selected element is itself `null` or the value-type default.

## Create a set

```csharp
HashSet<string> names = values.ToHashSet(StringComparer.OrdinalIgnoreCase);
```

`ToHashSet()` returns a new set and removes duplicates according to the supplied comparer, or `EqualityComparer<T>.Default` when none is supplied. A null or empty list returns an empty set configured with that comparer. Later changes to the list and set are independent.

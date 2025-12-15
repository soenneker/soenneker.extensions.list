using Soenneker.Utils.Random;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Soenneker.Extensions.List;

/// <summary>
/// A collection of helpful list extension methods
/// </summary>
public static class ListExtension
{
    /// <summary>
    /// Replaces the first element in the list that matches the specified predicate with a new value.
    /// </summary>
    /// <remarks>Only the first element that matches the predicate is replaced. If no elements match, the list
    /// remains unchanged.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which to search for an element to replace. If the list is null or empty, the method does nothing.</param>
    /// <param name="match">The predicate used to locate the element to replace. Cannot be null.</param>
    /// <param name="newItem">The new value to assign to the first matching element.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="match"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Replace<T>(this List<T>? list, Predicate<T> match, T newItem)
    {
        if (list is null || list.Count == 0)
            return;

        if (match is null)
            throw new ArgumentNullException(nameof(match));

        Span<T> span = CollectionsMarshal.AsSpan(list);

        for (int i = 0; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                span[i] = newItem;
                return;
            }
        }
    }

    /// <summary>
    /// Removes the first element from the list that matches the conditions defined by the specified predicate.
    /// </summary>
    /// <remarks>Only the first element that matches the predicate is removed. If no elements match, the list
    /// remains unchanged.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which to remove the first matching element. If the list is null or empty, the method does nothing.</param>
    /// <param name="match">The predicate that defines the conditions of the element to remove. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="match"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Remove<T>(this List<T>? list, Predicate<T> match)
    {
        if (list is null || list.Count == 0)
            return;

        if (match is null)
            throw new ArgumentNullException(nameof(match));

        Span<T> span = CollectionsMarshal.AsSpan(list);

        for (int i = 0; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                list.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// Randomly reorders the elements in the specified list in place. Threadsafe.
    /// </summary>
    /// <remarks>This method modifies the input list directly and does not create a new collection. The
    /// shuffle is performed using a variant of the Fisher–Yates algorithm, ensuring each possible permutation is
    /// equally likely. The operation is efficient for both List<T> and other IList<T> implementations.</remarks>
    /// <typeparam name="T">The type of elements in the list to shuffle.</typeparam>
    /// <param name="list">The list whose elements will be shuffled. If null or contains fewer than two elements, the method does nothing.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Shuffle<T>(this IList<T>? list)
    {
        if (list is null)
            return;

        int n = list.Count;
        if (n < 2)
            return;

        if (list is List<T> l)
        {
            ShuffleList(l);
            return;
        }

        while (n > 1)
        {
            int k = RandomUtil.Next(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShuffleList<T>(List<T> list)
    {
        Span<T> span = CollectionsMarshal.AsSpan(list);

        int n = span.Length;
        while (n > 1)
        {
            int k = RandomUtil.Next(n);
            n--;
            (span[n], span[k]) = (span[k], span[n]);
        }
    }

    /// <summary>
    /// Shuffles the list using a cryptographically strong number.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SecureShuffle<T>(this IList<T>? list)
    {
        if (list is null)
            return;

        int n = list.Count;
        if (n < 2)
            return;

        if (list is List<T> l)
        {
            SecureShuffleList(l);
            return;
        }

        while (n > 1)
        {
            int k = RandomNumberGenerator.GetInt32(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SecureShuffleList<T>(List<T> list)
    {
        Span<T> span = CollectionsMarshal.AsSpan(list);

        int n = span.Length;
        while (n > 1)
        {
            int k = RandomNumberGenerator.GetInt32(n);
            n--;
            (span[n], span[k]) = (span[k], span[n]);
        }
    }

    /// <summary>
    /// Returns a random element from the specified list, or the default value if the list is null or empty.
    /// </summary>
    /// <remarks>If the list contains exactly one element, that element is always returned. If the list is
    /// null or empty, the method returns the default value for type T.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which to select a random element. Can be null.</param>
    /// <returns>A randomly selected element from the list, or the default value for type T if the list is null or contains no
    /// elements.</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetRandom<T>(this IList<T>? list)
    {
        if (list is null)
            return default;

        int count = list.Count;

        if (count == 0)
            return default;

        if (count == 1)
            return list[0];

        return list[RandomUtil.Next(count)];
    }

    /// <summary>
    /// Creates a new HashSet<T> containing the elements of the specified list.
    /// </summary>
    /// <remarks>The order of elements in the resulting set is not guaranteed. Duplicate elements in the list
    /// are ignored in the resulting set. The returned HashSet<T> is independent of the original list; changes to one do
    /// not affect the other.</remarks>
    /// <typeparam name="T">The type of elements in the list and resulting set.</typeparam>
    /// <param name="list">The list whose elements are copied to the new HashSet<T>. Can be null or empty.</param>
    /// <param name="comparer">The equality comparer to use for the set. If null, the default equality comparer for type T is used.</param>
    /// <returns>A HashSet<T> containing the elements of the list. Returns an empty set if the list is null or empty.</returns>
    [Pure]
    public static HashSet<T> ToHashSet<T>(this IList<T>? list, IEqualityComparer<T>? comparer = null)
    {
        if (list is null || list.Count == 0)
            return comparer is null ? [] : new HashSet<T>(comparer);

        var set = comparer is null ? new HashSet<T>(list.Count) : new HashSet<T>(list.Count, comparer);

        if (list is List<T> l)
        {
            Span<T> span = CollectionsMarshal.AsSpan(l);
            for (int i = 0; i < span.Length; i++)
                set.Add(span[i]);

            return set;
        }

        for (int i = 0; i < list.Count; i++)
            set.Add(list[i]);

        return set;
    }


    /// <summary>
    /// Removes all elements from the list that match the conditions defined by the specified predicate and state
    /// object.
    /// </summary>
    /// <remarks>The method preserves the order of the remaining elements. The predicate function can use the
    /// state object to perform context-sensitive matching.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which elements will be removed. Cannot be null.</param>
    /// <param name="match">A function that defines the conditions of the elements to remove. The function receives an element and the state
    /// object, and returns <see langword="true"/> to remove the element; otherwise, <see langword="false"/>.</param>
    /// <param name="state">An object containing state information to be used by the predicate, or <see langword="null"/> to ignore state.</param>
    /// <returns>The number of elements removed from the list.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RemoveAll<T>(this List<T> list, Func<T, object?, bool> match, object? state)
    {
        if (list is null || list.Count == 0)
            return 0;

        if (match is null)
            throw new ArgumentNullException(nameof(match));

        int count = list.Count;

        int freeIndex = 0;
        while ((uint)freeIndex < (uint)count && !match(list[freeIndex], state))
            freeIndex++;

        if (freeIndex >= count)
            return 0;

        int current = freeIndex + 1;

        while (current < count)
        {
            while (current < count && match(list[current], state))
                current++;

            if (current < count)
                list[freeIndex++] = list[current++];
        }

        int removed = count - freeIndex;
        list.RemoveRange(freeIndex, removed);
        return removed;
    }
}
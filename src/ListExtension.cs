using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using Soenneker.Extensions.Enumerable;
using Soenneker.Utils.Random;

namespace Soenneker.Extensions.List;

/// <summary>
/// A collection of helpful list extension methods
/// </summary>
public static class ListExtension
{
    /// <summary>
    /// Finds a particular item in the list using a predicate, and replaces it
    /// </summary>
    public static void Replace<T>(this List<T> list, Predicate<T> oldItemSelector, T newItem)
    {
        //TODO: check for different situations here and throw exception
        //if list contains multiple items that match the predicate
        //or check for nullability of list and etc ...

        if (list.Empty())
            return;

        int oldItemIndex = list.FindIndex(oldItemSelector);

        if (oldItemIndex != -1)
            list[oldItemIndex] = newItem;
    }

    /// <summary>
    /// Removes a particular item in the list using a predicate
    /// </summary>
    public static void Remove<T>(this List<T> list, Predicate<T> oldItemSelector)
    {
        //TODO: check for different situations here and throw exception
        //if list contains multiple items that match the predicate
        //or check for nullability of list and etc ...

        if (list.Empty())
            return;

        int oldItemIndex = list.FindIndex(oldItemSelector);

        if (oldItemIndex != -1)
            list.RemoveAt(oldItemIndex);
    }

    /// <summary>
    /// Simple foreach over toRemove list, removing each from the target. This method is safe; no exceptions will be thrown if either list is null or empty.
    /// </summary>
    public static void RemoveEnumerableFromList<T>(this IList<T>? list, IEnumerable<T>? toRemove)
    {
        if (list.IsNullOrEmpty())
            return;

        if (toRemove.IsNullOrEmpty())
            return;

        foreach (T item in toRemove)
        {
            list.Remove(item);
        }
    }

    /// <inheritdoc cref="RemoveEnumerableFromList{T}"/>
    public static void RemoveFromList<T>(this IList<T> list, params T[] toRemove)
    {
        RemoveEnumerableFromList(list, toRemove);
    }

    /// <summary>
    /// Shuffles the list using a random number. Not cryptographically strong, but thread safe.
    /// </summary>
    public static void Shuffle<T>(this IList<T>? list)
    {
        if (list == null || list.Count < 2)
            return;

        int n = list.Count;

        while (n > 1)
        {
            int k = RandomUtil.Next(n--);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary>
    /// Shuffles the list using a cryptographically strong number.
    /// </summary>
    public static void SecureShuffle<T>(this IList<T>? list)
    {
        if (list == null || list.Count < 2)
            return;

        int n = list.Count;

        while (n > 1)
        {
            int k = RandomNumberGenerator.GetInt32(n--);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    [Pure]
    public static T? GetRandom<T>(this IList<T>? list)
    {
        if (list.IsNullOrEmpty())
            return default;

        if (list.Count == 1)
            return list[0];

        int index = RandomUtil.Next(0, list.Count);

        T result = list[index];
        return result;
    }
}
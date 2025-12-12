using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
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

        if (list is null || list.Count == 0)
            return;

        int oldItemIndex = list.FindIndex(oldItemSelector);

        if (oldItemIndex >= 0)
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

        if (list is null || list.Count == 0)
            return;

        int oldItemIndex = list.FindIndex(oldItemSelector);

        if (oldItemIndex >= 0)
            list.RemoveAt(oldItemIndex);
    }

    /// <summary>
    /// Shuffles the list using a random number. Not cryptographically strong, but thread safe.
    /// </summary>
    public static void Shuffle<T>(this IList<T>? list)
    {
        if (list is null || list.Count < 2)
            return;

        int n = list.Count;

        while (n > 1)
        {
            int k = RandomUtil.Next(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary>
    /// Shuffles the list using a cryptographically strong number.
    /// </summary>
    public static void SecureShuffle<T>(this IList<T>? list)
    {
        if (list is null || list.Count < 2)
            return;

        int n = list.Count;

        while (n > 1)
        {
            int k = RandomNumberGenerator.GetInt32(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    [Pure]
    public static T? GetRandom<T>(this IList<T>? list)
    {
        if (list is not null && list.Count > 0)
        {
            // If there's exactly one item, return it
            if (list.Count == 1)
                return list[0];

            // Otherwise, return a random item
            return list[RandomUtil.Next(0, list.Count)];
        }

        // Return default value if list is null or empty
        return default;
    }

    /// <summary>
    /// Converts an <see cref="IList{T}"/> to a <see cref="HashSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to convert. If null, an empty <see cref="HashSet{T}"/> is returned.</param>
    /// <returns>A <see cref="HashSet{T}"/> containing the elements of the list.</returns>
    [Pure]
    public static HashSet<T> ToHashSet<T>(this IList<T> list)
    {
        if (list is null || list.Count == 0)
            return [];

        var result = new HashSet<T>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            result.Add(list[i]);
        }
        return result;
    }
}
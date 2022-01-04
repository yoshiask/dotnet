// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with arrays.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Yields a column from a jagged array.
    /// An exception will be thrown if the column is out of bounds, and return default in places where there are no elements from inner arrays.
    /// Note: There is no equivalent GetRow method, as you can use array[row] to retrieve.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="rectarray">The source array.</param>
    /// <param name="column">Column record to retrieve, 0-based index.</param>
    /// <returns>Yielded enumerable of column elements for given column, and default values for smaller inner arrays.</returns>
    public static IEnumerable<T?> GetColumn<T>(this T?[][] rectarray, int column)
    {
        if (column < 0 || column >= rectarray.Max(array => array.Length))
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        for (int r = 0; r < rectarray.GetLength(0); r++)
        {
            if (column >= rectarray[r].Length)
            {
                yield return default;

                continue;
            }

            yield return rectarray[r][column];
        }
    }

    /// <summary>
    /// Returns a simple string representation of an array.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="array">The source array.</param>
    /// <returns>The <see cref="string"/> representation of the array.</returns>
    public static string ToArrayString<T>(this T?[] array)
    {
        // The returned string will be in the following format:
        // [1, 2, 3]
        StringBuilder builder = new();

        _ = builder.Append('[');

        for (int i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                _ = builder.Append(",\t");
            }

            _ = builder.Append(array[i]?.ToString());
        }

        _ = builder.Append(']');

        return builder.ToString();
    }

    /// <summary>
    /// Returns a simple string representation of a jagged array.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="mdarray">The source array.</param>
    /// <returns>String representation of the array.</returns>
    public static string ToArrayString<T>(this T?[][] mdarray)
    {
        // The returned string uses the same format as the overload for 2D arrays
        StringBuilder builder = new();

        _ = builder.Append('[');

        for (int i = 0; i < mdarray.Length; i++)
        {
            if (i != 0)
            {
                _ = builder.Append(',');
                _ = builder.Append(Environment.NewLine);
                _ = builder.Append(' ');
            }

            _ = builder.Append('[');

            T?[] row = mdarray[i];

            for (int j = 0; j < row.Length; j++)
            {
                if (j != 0)
                {
                    _ = builder.Append(",\t");
                }

                _ = builder.Append(row[j]?.ToString());
            }

            _ = builder.Append(']');
        }

        _ = builder.Append(']');

        return builder.ToString();
    }

    /// <summary>
    /// Returns a simple string representation of a 2D array.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="array">The source array.</param>
    /// <returns>The <see cref="string"/> representation of the array.</returns>
    public static string ToArrayString<T>(this T?[,] array)
    {
        // The returned string will be in the following format:
        // [[1, 2,  3],
        //  [4, 5,  6],
        //  [7, 8,  9]]
        StringBuilder builder = new();

        _ = builder.Append('[');

        int height = array.GetLength(0);
        int width = array.GetLength(1);

        for (int i = 0; i < height; i++)
        {
            if (i != 0)
            {
                _ = builder.Append(',');
                _ = builder.Append(Environment.NewLine);
                _ = builder.Append(' ');
            }

            _ = builder.Append('[');

            for (int j = 0; j < width; j++)
            {
                if (j != 0)
                {
                    _ = builder.Append(",\t");
                }

                _ = builder.Append(array[i, j]?.ToString());
            }

            _ = builder.Append(']');
        }

        _ = builder.Append(']');

        return builder.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyCollection{T}"/>
    /// class that is a read-only wrapper around the specified list.
    /// </summary>
    /// <param name="array">The <see cref="IList{T}"/> to wrap.</param>
    public static
#if !NET35
        ReadOnlyCollection<T>
#else
        Collections.ReadOnlyCollection<T>
#endif
        AsReadOnlyPolyfill<T>(this IList<T> array)
    {
        return new(array);
    }

#if NET35
    /// <summary>
    /// Forms a slice out of the current read-only span starting at a specified index for a specified length.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="start">The index at which to begin this slice.</param>
    /// <param name="length">The desired length for the slice.</param>
    /// <returns>A read-only span that consists of length elements from the current span starting at start.</returns>
    /// <exception cref="ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
    public static T[] Slice<T>(this ICollection<T> array, int start, int length)
    {
        T?[] slice = new T?[length];
        array.CopyTo(slice, start);
        return slice;
    }
#endif
}

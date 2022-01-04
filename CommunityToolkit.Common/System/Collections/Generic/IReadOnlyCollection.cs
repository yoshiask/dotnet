// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35

namespace System.Collections.Generic;

/// <summary>
/// Represents a strongly-typed, read-only collection of elements.
/// </summary>
/// <typeparam name="T">
/// The type of the elements.
/// </typeparam>
public interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
{
    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    int Count { get; }
}

#endif
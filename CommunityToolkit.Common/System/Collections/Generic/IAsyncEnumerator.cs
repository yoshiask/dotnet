// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35

using System.Threading.Tasks;

namespace System.Collections.Generic;

/// <summary>
/// Supports a simple asynchronous iteration over a generic collection.
/// </summary>
/// <typeparam name="T">
/// The type of objects to enumerate.
/// </typeparam>
/// <remarks>
/// Because <c>ValueTask</c> is not available in .NET Framework 3.5,
/// <see cref="Task"/> is used instead. This may result in more
/// heap allocations than the BCL implementation.
/// </remarks>
public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    /// <summary>Advances the enumerator asynchronously to the next element of the collection.</summary>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if the enumerator
    /// was successfully advanced to the next element, or <c>false</c> if the enumerator has passed the end
    /// of the collection.
    /// </returns>
    Task<bool> MoveNextAsync();

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    T Current { get; }
}

#endif

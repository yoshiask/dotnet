// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35

using System.Threading;

namespace System.Collections.Generic;

/// <summary>
/// Exposes an enumerator that provides asynchronous iteration over values of a specified type.
/// </summary>
/// <typeparam name="T">
/// The type of values to enumerate.
/// </typeparam>
/// <remarks>
/// Because <c>ValueTask</c> is not available in .NET Framework 3.5, internally
/// <see cref="Threading.Tasks.Task"/> is used instead. This may result in more
/// heap allocations than the BCL implementation.
/// </remarks>
public interface IAsyncEnumerable<out T>
{
    /// <summary>
    /// Returns an enumerator that iterates asynchronously through the collection.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous iteration.
    /// </param>
    /// <returns>
    /// An enumerator that can be used to iterate asynchronously through the collection.
    /// </returns>
    IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
}

#endif
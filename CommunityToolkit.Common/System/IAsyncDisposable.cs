// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35

using System.Threading.Tasks;

namespace System;

/// <summary>
/// Provides a mechanism for releasing unmanaged resources asynchronously.
/// </summary>
/// <remarks>
/// Because <c>ValueTask</c> is not available in .NET Framework 3.5,
/// <see cref="Task"/> is used instead. This may result in more
/// heap allocations than the BCL implementation.
/// </remarks>
public interface IAsyncDisposable
{
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.
    /// </summary>
    Task DisposeAsync();
}

#endif
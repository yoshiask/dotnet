// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class EnumeratorCancellationAttribute : Attribute
{
    public EnumeratorCancellationAttribute()
    {
    }
}

#endif
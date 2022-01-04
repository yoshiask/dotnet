// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET35

namespace CommunityToolkit.Common.Helpers;

/// <summary>
/// Represents an item available in a directory.
/// </summary>
public class DirectoryItem
{
    /// <summary>
    /// The type of the item.
    /// </summary>
    public DirectoryItemType ItemType { get; }
    
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string Name { get; }
}

#endif

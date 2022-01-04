using System;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with <see cref="Enum"/>s.
/// </summary>
public static class EnumExtensions
{
#if NET35
    /// <summary>
    /// Determines whether one or more bit fields are set in the current instance.
    /// </summary>
    /// <param name="flag">An enumeration value.</param>
    /// <returns>
    /// <c>true</c> if the bit field or bit fields that are set in flag are also set in the
    /// current instance; otherwise, <c>false</c>.
    /// </returns>
    public static unsafe bool HasFlag<TEnum>(this TEnum val, TEnum flag) where TEnum : unmanaged, Enum
    {
        ulong uVal = *(ulong*)&val;
        ulong uFlag = *(ulong*)&flag;
        return (uVal & uFlag) > 0;
    }
#endif
}

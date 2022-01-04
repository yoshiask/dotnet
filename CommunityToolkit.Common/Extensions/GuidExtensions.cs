using System;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with <see cref="Guid"/>s.
/// </summary>
public class GuidExtensions
{
    /// <summary>
    /// Converts the string representation of a GUID to the equivalent System.Guid structure.
    /// </summary>
    /// <param name="input">
    /// The GUID to convert.
    /// </param>
    /// <param name="result">
    /// The structure that will contain the parsed value. If the method returns <c>true</c>,
    /// result contains a valid <see cref="Guid"/>. If the method returns <c>false</c>, result equals <see cref="Guid.Empty"/>.
    /// </param>
    /// <remarks>
    /// Use <see cref="Guid.TryParse"/> when available.
    /// </remarks>
    public static bool TryParse(string? input, out Guid result)
    {
#if NET35
        try
        {
            result = new(input);
            return true;
        }
        catch
        {
            result = Guid.Empty;
            return false;
        }
#else
        return Guid.TryParse(input, out result);
#endif
    }
}

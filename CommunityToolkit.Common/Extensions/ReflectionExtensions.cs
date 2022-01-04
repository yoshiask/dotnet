#if NET35

using System.Reflection;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Returns the property value of a specified object.
    /// </summary>
    /// <param name="prop">The property whose value will be returned.</param>
    /// <param name="obj">The object whose property value will be returned.</param>
    /// <returns>The property value of the specified object.</returns>
    public static object? GetValue(this PropertyInfo prop, object? obj)
    {
        return prop.GetValue(obj, BindingFlags.Default, null, null, null);
    }

    /// <summary>
    /// Sets the property value of a specified object.
    /// </summary>
    /// <param name="prop">The property whose value will be set.</param>
    /// <param name="obj">The object whose property value will be set.</param>
    /// <param name="value">The new property value.</param>
    public static void SetValue(this PropertyInfo prop, object? obj, object? value)
    {
        prop.SetValue(obj, value, BindingFlags.Default, null, null, null);
    }
}

#endif

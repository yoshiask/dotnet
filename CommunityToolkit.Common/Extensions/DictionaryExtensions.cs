#if NET35

using System;
using System.Collections.Generic;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with dictionaries.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Atomically searches for a specified key in the table and returns the corresponding
    /// value. If the key does not exist in the table, the method invokes a callback
    /// method to create a value that is bound to the specified key.
    /// </summary>
    /// <param name="key">
    /// The key to search for. key represents the object to which the property is attached.
    /// </param>
    /// <param name="createValueCallback">
    /// A delegate to a method that can create a value for the given key. It has a single
    /// parameter of type TKey, and returns a value of type TValue.
    /// </param>
    /// <returns>
    /// The value attached to key, if key already exists in the table; otherwise, the
    /// new value returned by the createValueCallback delegate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// key or createValueCallback is null.
    /// </exception>
    public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> createValueCallback)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        if (createValueCallback == null)
            throw new ArgumentNullException(nameof(createValueCallback));

        TValue value;
        if (dict.TryGetValue(key, out value))
            return value;

        value = createValueCallback(key);
        dict.Add(key, value);
        return value;
    }
}

#endif

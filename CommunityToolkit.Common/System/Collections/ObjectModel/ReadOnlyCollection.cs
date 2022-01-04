// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET35

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CommunityToolkit.Common.Collections;

[Serializable]
[DebuggerDisplay("Count = {Count}")]
public class ReadOnlyCollection<T> : IList<T>, IList, IReadOnlyList<T>
{
    private static readonly NotSupportedException readOnlyExcpetion = new("Collection is read-only.");

    private readonly IList<T> list; // Do not rename (binary serialization)

    public ReadOnlyCollection(IList<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("list");
        }
        this.list = list;
    }

    public int Count => list.Count;

    public T this[int index] => list[index];

    public bool Contains(T value)
    {
        return list.Contains(value);
    }

    public void CopyTo(T[] array, int index)
    {
        list.CopyTo(array, index);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public int IndexOf(T value)
    {
        return list.IndexOf(value);
    }

    protected IList<T> Items => list;

    bool ICollection<T>.IsReadOnly => true;

    T IList<T>.this[int index]
    {
        get => list[index];
        set => throw readOnlyExcpetion;
    }

    void ICollection<T>.Add(T value)
    {
        throw readOnlyExcpetion;
    }

    void ICollection<T>.Clear()
    {
        throw readOnlyExcpetion;
    }

    void IList<T>.Insert(int index, T value)
    {
        throw readOnlyExcpetion;
    }

    bool ICollection<T>.Remove(T value)
    {
        throw readOnlyExcpetion;
        return false;
    }

    void IList<T>.RemoveAt(int index)
    {
        throw readOnlyExcpetion;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list).GetEnumerator();
    }

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => list is ICollection coll ? coll.SyncRoot : this;

    void ICollection.CopyTo(Array array, int index)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }

        if (array.Rank != 1)
        {
            throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
        }

        if (array.GetLowerBound(0) != 0)
        {
            throw new ArgumentException("The lower bound of target array must be zero.");
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("Non-negative number required.");
        }

        if (array.Length - index < Count)
        {
            throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
        }

        if (array is T[] items)
        {
            list.CopyTo(items, index);
        }
        else
        {
            //
            // Catch the obvious case assignment will fail.
            // We can't find all possible problems by doing the check though.
            // For example, if the element type of the Array is derived from T,
            // we can't figure out if we can successfully copy the element beforehand.
            //
            Type targetType = array.GetType().GetElementType()!;
            Type sourceType = typeof(T);
            if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
            {
                goto invalidArrayType;
            }

            //
            // We can't cast array of value type to object[], so we don't support
            // widening of primitive types here.
            //
            object?[]? objects = array as object[];
            if (objects == null)
            {
                goto invalidArrayType;
            }

            int count = list.Count;
            try
            {
                for (int i = 0; i < count; i++)
                {
                    objects[index++] = list[i];
                }
            }
            catch (ArrayTypeMismatchException)
            {
                goto invalidArrayType;
            }

            invalidArrayType:
            throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
        }
    }

    bool IList.IsFixedSize => true;

    bool IList.IsReadOnly => true;

    object? IList.this[int index]
    {
        get => list[index];
        set => throw readOnlyExcpetion;
    }

    int IList.Add(object? value)
    {
        throw readOnlyExcpetion;
        return -1;
    }

    void IList.Clear()
    {
        throw readOnlyExcpetion;
    }

    private static bool IsCompatibleObject(object? value)
    {
        // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
        // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
        return (value is T) || (value == null && default(T) == null);
    }

    bool IList.Contains(object? value)
    {
        if (IsCompatibleObject(value))
        {
            return Contains((T)value!);
        }
        return false;
    }

    int IList.IndexOf(object? value)
    {
        if (IsCompatibleObject(value))
        {
            return IndexOf((T)value!);
        }
        return -1;
    }

    void IList.Insert(int index, object? value)
    {
        throw readOnlyExcpetion;
    }

    void IList.Remove(object? value)
    {
        throw readOnlyExcpetion;
    }

    void IList.RemoveAt(int index)
    {
        throw readOnlyExcpetion;
    }

    public static implicit operator ReadOnlyCollection<T>(T[] array) => new(array);
    public static implicit operator ReadOnlyCollection<T>(List<T> list) => new(list);
    public static implicit operator ReadOnlyCollection<T>(Collection<T> collection) => new(collection);
}

#endif

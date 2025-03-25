namespace System;

public static class TupleArrayExtensions
{
    /// <summary>
    /// Retrieves the tuple at the specified index in the array.
    /// </summary>
    /// <typeparam name="TKey">The type of the first item in the tuple.</typeparam>
    /// <typeparam name="TValue">The type of the second item in the tuple.</typeparam>
    /// <param name="tuples">The array of tuples.</param>
    /// <param name="index">The index to retrieve.</param>
    /// <returns>The tuple at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
    public static (TKey, TValue) GetAtIndex<TKey, TValue>(this (TKey, TValue)[] tuples, int index)
    {
        if (index < 0 || index >= tuples.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        return tuples[index];
    }

    /// <summary>
    /// Searches for a tuple by its first item (key) and returns the associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the first item in the tuple.</typeparam>
    /// <typeparam name="TValue">The type of the second item in the tuple.</typeparam>
    /// <param name="tuples">The array of tuples.</param>
    /// <param name="key">The key to search for.</param>
    /// <param name="value">The associated value if found; default if not found.</param>
    /// <returns>True if the key was found; false otherwise.</returns>
    public static bool FindByKey<TKey, TValue>(this (TKey, TValue)[] tuples, TKey key, out TValue? value)
    {
        foreach (var tuple in tuples)
        {
            if (EqualityComparer<TKey>.Default.Equals(tuple.Item1, key))
            {
                value = tuple.Item2;
                return true;
            }
        }
        value = default;
        return false;
    }

    /// <summary>
    /// Searches for a tuple by its first item (key) and returns the entire tuple.
    /// </summary>
    /// <typeparam name="TKey">The type of the first item in the tuple.</typeparam>
    /// <typeparam name="TValue">The type of the second item in the tuple.</typeparam>
    /// <param name="tuples">The array of tuples.</param>
    /// <param name="key">The key to search for.</param>
    /// <returns>The tuple if found; default if not found.</returns>
    public static (TKey, TValue)? FindTupleByKey<TKey, TValue>(this (TKey, TValue)[] tuples, TKey key)
    {
        foreach (var tuple in tuples)
        {
            if (EqualityComparer<TKey>.Default.Equals(tuple.Item1, key))
            {
                return tuple;
            }
        }
        return null;
    }
}
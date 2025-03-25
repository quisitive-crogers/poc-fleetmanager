namespace System;

/// <summary>
/// Extensions for System.Type.
/// </summary>
public static class TypeExtensions
{

    /// <summary>
    /// Returns true if the type is a Date/Time variable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if [is date time] [the specified type]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDateTime(this Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(DateTime?) ||
               type == typeof(DateTimeOffset?);
    }

    /// <summary>
    /// Returns true if the type is an integer type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the specified type is integer; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInteger(this Type type)
    {
        if (type.IsValueType)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(byte) || type == typeof(short)
                   || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong)
                   || type == typeof(int?) || type == typeof(long?) || type == typeof(byte?) ||
                   type == typeof(short?)
                   || type == typeof(ushort?) || type == typeof(uint?) || type == typeof(ulong?);
        }

        return false;
    }

    /// <summary>
    /// Returns TRUE of the type is a floating point value.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the specified type is float; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFloat(this Type type)
    {
        if (type.IsValueType)
        {
            return type == typeof(double) || type == typeof(float) || type == typeof(decimal)
                   || type == typeof(double?) || type == typeof(float?) || type == typeof(decimal?);
        }

        return false;
    }

    /// <summary>
    /// Determines whether this type is nullable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;
}
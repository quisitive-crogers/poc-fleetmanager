using System.Text;

namespace System;

/// <summary>
/// Extension methods for bytes. 
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Bytes the array as a string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string ByteArrayToString(this byte[] value)
    {
        return Encoding.ASCII.GetString(value);
    }

    /// <summary>
    /// Converts to stream.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static Stream ToStream(this byte[] value)
    {
        var stream = new MemoryStream();
        stream.Write(value, 0, value.Length);
        stream.Position = 0;
        return stream;
    }

}
using System.Text;

namespace System;
public static class ByteArrayExtensions
{
    public static T? ToObject<T>(this byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        return str.ToObject<T>();
    }
}
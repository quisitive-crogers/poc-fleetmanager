using System.Text.Json;

namespace System.IO;

/// <summary>
/// Extension methods for streams.
/// </summary>
public static class StreamExtensions
{
    /// <summary>Converts a stream to a bytearray..</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static byte[] ToByteArray(this Stream input)
    {
        var buffer = new byte[16 * 1024];
        using var ms = new MemoryStream();
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            ms.Write(buffer, 0, read);
        }

        return ms.ToArray();
    }

    /// <summary>Converts a stream to a byte array.</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static async Task<byte[]> ToByteArrayAsync(this Stream input)
    {
        var buffer = new byte[16 * 1024];
        using var ms = new MemoryStream();
        int read;
        while ((read = await input.ReadAsync(buffer)) > 0)
        {
            await ms.WriteAsync(buffer.AsMemory(0, read));
        }

        return ms.ToArray();
    }


    /// <summary>
    /// Reads a source stream and deserializes it from compressed JSON.
    /// </summary>
    /// <typeparam name="TData">The data type of the JSON data</typeparam>
    /// <param name="sourceStream">The source stream containing the GZip'd JSON</param>
    /// <returns></returns>
    public static TData? ReadAsJson<TData>(this Stream sourceStream)
    {
        using StreamReader reader = new(sourceStream);
        var jsonData = reader.ReadToEnd();
        return JsonSerializer.Deserialize<TData>(jsonData);
    }
}
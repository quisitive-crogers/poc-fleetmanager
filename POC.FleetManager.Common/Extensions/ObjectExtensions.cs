using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;

namespace System;

/// <summary>
/// Object extension methods. 
/// </summary>
public static class ObjectExtensions
{
    public static string GetTargetPlatform(this object anyObject)
    {
        var targetPlatform = "Unknown";
        var targetPlatformAttributeType = typeof(TargetPlatformAttribute);
        var targetPlatformAttributes = anyObject.GetType().Assembly.GetCustomAttributes(targetPlatformAttributeType, true);
        if (targetPlatformAttributes.Length > 0)
        {
            var targetPlatformAttribute = (TargetPlatformAttribute)targetPlatformAttributes.First();
            targetPlatform = (targetPlatformAttribute.PlatformName);
        }

        return targetPlatform!;
    }
    public static string GetTargetFramework(this object anyObject)
    {
        var targetFramework = "Unknown";
        var targetFrameworkAttributeType = typeof(TargetFrameworkAttribute);
        var targetFrameworkAttributes = anyObject.GetType().Assembly.GetCustomAttributes(targetFrameworkAttributeType, true);
        if (targetFrameworkAttributes.Length > 0)
        {
            var targetFrameworkAttribute = (TargetFrameworkAttribute)targetFrameworkAttributes.First();
            targetFramework = (targetFrameworkAttribute.FrameworkDisplayName);
        }

        return targetFramework!;
    }

    /// <summary>
    /// Sets null string property values to an empty string (rather than NULL).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classToConvert">The class to convert.</param>
    public static void SetClassNullStringPropertiesToEmptyString<T>(this T classToConvert) where T : class
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (var info in properties)
        {
            // if a string and null, set to String.Empty
            if (info.PropertyType == typeof(string) &&
                info.GetValue(classToConvert, null) == null)
            {
                info.SetValue(classToConvert, String.Empty, null);
            }
        }
    }


    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static string ToJsonString(this object source)
    {
        return JsonSerializer.Serialize(source);
    }

    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static T? ToObject<T>(this string? source)
    {
        if (source.IsNullOrWhiteSpace()) return default;

        return JsonSerializer.Deserialize<T>(source!);
    }

    public static byte[] ToBytes(this object obj)
    {
        var jsonStr = obj.ToJsonString();
        return Encoding.UTF8.GetBytes(jsonStr);
    }

    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="targetStream">The target stream to write to.</param>
    /// <returns></returns>
    public static void SaveJson(this object source, Stream targetStream)
    {
        using var streamWriter = new StreamWriter(targetStream, Encoding.Default, 4096, true);
        streamWriter.Write(source.ToJsonString());
        streamWriter.Flush();
    }

    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="targetStream">The target stream to write to.</param>
    /// <returns></returns>
    public static async Task SaveJsonAsync(this object source, Stream targetStream)
    {
        await using var streamWriter = new StreamWriter(targetStream, Encoding.Default, 4096, true);
        await streamWriter.WriteAsync(source.ToJsonString());
        await streamWriter.FlushAsync();
    }

    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="filePath">The path of the file to write.</param>
    /// <remarks>
    /// If a file exists at the target path, it will be overwritten.
    /// </remarks>
    public static void SaveJson(this object source, string filePath)
    {
        using var fileStream = File.Create(filePath);
        source.SaveJson(fileStream);
    }

    /// <summary>
    /// Converts to jsonstring.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="filePath">The path of the file to write.</param>
    /// <returns></returns>
    public static Task SaveJsonAsync(this object source, string filePath)
    {
        using var fileStream = File.Create(filePath);
        return source.SaveJsonAsync(fileStream);
    }

    /// <summary>
    /// Checks for an optional dependency. Creates a default if not found.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency.</typeparam>
    /// <param name="inputDependency">The dependency passed in the constructor.</param>
    /// <param name="defaultInstanceFactory">The factory for the default instance.</param>
    /// <returns>
    /// The dependency if not null. Exception is thrown if null. 
    /// </returns>
    public static TDependency OptionalDependency<TDependency>(this TDependency? inputDependency,
        Func<TDependency> defaultInstanceFactory)
    {
        if (inputDependency != null)
        {
            return inputDependency;
        }

        return defaultInstanceFactory();
    }

    /// <summary>
    /// Checks for an optional dependency. Creates a default if not found.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency.</typeparam>
    /// <param name="inputDependency">The dependency passed in the constructor.</param>
    /// <returns>
    /// The dependency if not null. Exception is thrown if null. 
    /// </returns>
    public static TDependency OptionalDependency<TDependency>(this TDependency inputDependency)
        where TDependency : new()
    {
        return OptionalDependency(inputDependency, () => new TDependency());
    }


    private static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday
               || date.DayOfWeek == DayOfWeek.Sunday;
    }


    /// <summary>
    /// Returns a pretty name for a type. 
    /// </summary>
    /// <param name="typ"></param>
    /// <param name="fullNames">True to include the full qualified namespece names</param>
    /// <returns></returns>
    public static string GetDisplayName(this object typ)
    {
        return typ.GetType().Name;
    }
}
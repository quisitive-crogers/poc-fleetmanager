using System.Reflection;
using System.Text;

namespace System.Collections.Generic;

/// <summary>
/// Extension methods for Lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Takes a list of objects and converts it to a Csv string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string GetCsv<T>(this List<T> list)
    {
        var csvStringBuilder = new StringBuilder();

        //Get the properties for type T for the headers
        PropertyInfo[] propInfos = typeof(T).GetProperties();
        for (int i = 0; i <= propInfos.Length - 1; i++)
        {
            csvStringBuilder.Append(propInfos[i].Name);

            if (i < propInfos.Length - 1)
            {
                csvStringBuilder.Append(',');
            }
        }

        csvStringBuilder.AppendLine();

        //Loop through the collection, then the properties and add the values
        for (int i = 0; i <= list.Count - 1; i++)
        {
            var item = list[i];
            WriteItem(propInfos, item, csvStringBuilder);
        }

        return csvStringBuilder.ToString();
    }

    private static void WriteItem<T>(PropertyInfo[] properties, T item, StringBuilder csvStringBuilder)
    {
        for (int j = 0; j <= properties.Length - 1; j++)
        {
            if (item == null)
            {
                continue;
            }

            object? o = item.GetType()!.GetProperty(properties[j].Name)!.GetValue(item, null);

            if (o != null)
            {
                string value = o.ToString() ?? "";

                value = value.Replace("\"", "'");

                //Check if the value contans a comma and place it in quotes if so
                if (value.Contains(','))
                {
                    value = string.Concat("\"", value, "\"");
                }

                //Replace any \r or \n special characters from a new line with a space
                if (value.Contains('\r'))
                {
                    value = value.Replace("\r", " ");
                }

                if (value.Contains('\n'))
                {
                    value = value.Replace("\n", " ");
                }

                csvStringBuilder.Append(value);
            }

            if (j < properties.Length - 1)
            {
                csvStringBuilder.Append(',');
            }
        }

        csvStringBuilder.AppendLine();
    }

    /// <summary>
    /// Gets distinct elements by a provided key selector. 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="keySelector">The key selector.</param>
    /// <returns></returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = [];
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public static string ToJoinedString<TSource, TKey>(this IEnumerable<TSource> source, string deliminator, Func<TSource, TKey> stringSelector)
    {
        var list = new List<string>();

        foreach (var item in source)
        {
            var newItem = stringSelector(item)?.ToString();
            list.Add(newItem ?? "");
        }

        return string.Join(deliminator, list);
    }

    public static bool None<TSource>(this IEnumerable<TSource> source)
    {
        return !source.Any();
    }

    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return !source.Any(predicate);
    }
}
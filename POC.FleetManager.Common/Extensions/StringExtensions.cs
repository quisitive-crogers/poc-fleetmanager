using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System;

/// <summary>
/// Class StringExtensions.
/// </summary>
public static partial class StringExtensions
{
    public static bool IsNotNullOrEmpty(this string? str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static bool IsNotNullOrWhiteSpace(this string? str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// To the stream.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>Stream.</returns>
    public static Stream ToStream(this string str)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(str);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Returns the value of a string or a default if the string is null or whitespace. 
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <param name="defaultValue">The default value if null or whitespace</param>
    /// <returns></returns>
    public static string ValueOrDefault(this string? str, string defaultValue)
    {
        return String.IsNullOrWhiteSpace(str) ? defaultValue : str;
    }

    /// <summary>
    /// To int value
    /// </summary>
    /// <param name="value">The string value to convert</param>
    /// <param name="failoverValue">A failover value to return if the string is not an int</param>
    /// <returns>An int</returns>
    public static int ToInt(this string value, int failoverValue = int.MinValue)
    {
        if (int.TryParse(value, out int retval)) return retval;

        return failoverValue;
    }

    /// <summary>
    /// To the bytes.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] ToBytes(this string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    /// <summary>
    /// To the base64.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string ToBase64(this string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Froms the base64.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string FromBase64(this string value)
    {
        byte[] bytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(bytes);
    }


    /// <summary>
    /// Nulls to empty string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string NullToEmptyString(this string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
    }

    /// <summary>
    /// URLs the encode.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string UrlEncode(this string value)
    {
        return HttpUtility.UrlEncode(value);
    }

    /// <summary>
    /// Decodes Html values. 
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string HtmlDecode(this string value)
    {
        return HttpUtility.HtmlDecode(value);
    }

    /// <summary>
    /// Html Encodes a string. 
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string HtmlEncode(this string value)
    {
        return HttpUtility.HtmlEncode(value);
    }

    /// <summary>
    /// Decodes a URL.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string UrlDecode(this string value)
    {
        return HttpUtility.UrlDecode(value);
    }

    /// <summary>
    /// Returns the just the numbers in a string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string GetNumbers(this string value)
    {
        return new string([.. value.Where(char.IsDigit)]);
    }

    /// <summary>
    /// Gets the last N characters in a string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="length">The length.</param>
    /// <returns>System.String.</returns>
    public static string? GetLast(this string source, int length)
    {
        if (string.IsNullOrEmpty(source))
            return null;
        if (length >= source.Length)
            return source;
        return source[^length..];
    }

    /// <summary>
    /// Makes sure a string is the specified length by either truncating or padding to the right.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="length">Desired length</param>
    /// <returns>System.String</returns>
    public static string FixedLengthPadRight(this string? value, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
        }

        if (value == null)
        {
            return string.Empty.PadRight(length);
        }

        int maxLength = Math.Min(value.Length, length);
        string returnValue = value[..maxLength];
        return returnValue.Length < length ? returnValue.PadRight(length) : returnValue;
    }

    /// <summary>
    /// Converts the string to a caption, inserting a space before each Upper
    /// Case character except:
    /// <list type="bullet">
    /// <item>an Upper Case character in the first position, or</item>
    /// <item>an Upper Case character whose preceding 2 characters are also Upper Case.</item>
    /// </list>
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>A string formatted as a caption.</returns>
    public static string ToCaption(this string s)
    {
        StringBuilder sb = new(s.Length);
        int index = 0;

        char lastChar = (char)0;
        foreach (char c in s)
        {
            // if the initial character is lower case, make it upper case
            if (index == 0)
            {
                sb.Append(Char.IsLower(c) ? Char.ToUpper(c) : c);
            }
            else
            // For any subsequent character, insert a space before:
            //   1) an upper case character if the prior character was lower case; or
            //   2) any non-letter character;
            // UNLESS there was already a space there
            {
                if (c == '_')
                    sb.Append(' ');
                else if (!Char.IsWhiteSpace(c))
                {
                    if (Char.IsNumber(c))
                    {
                        // If it's a number and the last char is not a number, add a space
                        if (!Char.IsNumber(lastChar))
                        {
                            sb.Append(' ');
                        }
                    }
                    else if (Char.IsLetter(c))
                    {
                        if ((Char.IsUpper(c)) && (!Char.IsUpper(s[index - 1])) && lastChar != '_')
                            sb.Append(' ');
                    }
                    else
                        sb.Append(' ');

                    sb.Append(c);
                }
            }

            lastChar = c;
            index++;
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts the string to a DNS safe string for use in things like Azure Container names
    /// </summary>
    /// <param name="s">The string to convert</param>
    /// <returns>A dns safe string</returns>
    public static string ToDNSName(this string s)
    {
        var workingString = s.ToLower();
        var retval = DNSRegEx().Replace(workingString, "");
        return retval;
    }


    /// <summary>
    /// Converts the string to an enum of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enumeration type to which to convert.</typeparam>
    /// <param name="s">The string to convert.</param>
    /// <param name="ignoreCase">Specifies whether the operation is case senstive.</param>
    /// <returns>An enumeration of type <typeparamref name="T"/>.</returns>
    public static T ToEnum<T>(this string s, bool ignoreCase = false)
    {
        return (T)Enum.Parse(typeof(T), s, ignoreCase);
    }


    /// <summary>
    /// Makes sure a string is the specified length by either truncating or padding to the right with specified character.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="length">Desired length</param>
    /// <param name="paddingChar">Character to pad with if the string isn't long enough</param>
    /// <returns>System.String</returns>
    public static string FixedLengthPadRight(this string? value, int length, char paddingChar)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
        }

        if (value == null)
        {
            return string.Empty.PadRight(length, paddingChar);
        }

        int maxLength = Math.Min(value.Length, length);
        string returnValue = value[..maxLength];
        return returnValue.Length < length ? returnValue.PadRight(length, paddingChar) : returnValue;
    }

    /// <summary>
    /// Makes sure a string is the specified length by either truncating or padding to the left.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="length">Desired length</param>
    /// <returns>System.String</returns>
    public static string FixedLengthPadLeft(this string? value, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
        }

        if (value == null)
        {
            return string.Empty.PadLeft(length);
        }

        int maxLength = Math.Min(value.Length, length);
        string returnValue = value[..maxLength];
        return returnValue.Length < length ? returnValue.PadLeft(length) : returnValue;
    }

    /// <summary>
    /// Makes sure a string is the specified length by either truncating or padding to the left with specified character.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="length">Desired length</param>
    /// <param name="paddingChar">Character to pad with if the string isn't long enough</param>
    /// <returns>System.String</returns>
    public static string FixedLengthPadLeft(this string? value, int length, char paddingChar)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
        }

        if (value == null)
        {
            return string.Empty.PadLeft(length, paddingChar);
        }

        int maxLength = Math.Min(value.Length, length);
        string returnValue = value[..maxLength];
        return returnValue.Length < length ? returnValue.PadLeft(length, paddingChar) : returnValue;
    }

    /// <summary>
    /// Removes the whitespace.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string RemoveWhitespace(this string value)
    {
        return new string([.. value.ToCharArray().Where(c => !char.IsWhiteSpace(c))]);
    }

    /// <summary>
    /// Converts all Diacritic characters in a string to their ASCII equivalent
    /// Courtesy: http://stackoverflow.com/a/13154805/476786
    /// A quick explanation:
    /// * Normalizing to form D splits characters like è to an e and a non spacing `
    /// * From this, the non spacing characters are removed
    /// * The result is normalized back to form C (I'm not sure if this is necessary)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertDiacriticToASCII(this string? value)
    {
        if (value == null) return String.Empty;
        var chars =
            value.Normalize(NormalizationForm.FormD)
                .ToCharArray()
                .Select(c => new { c, uc = CharUnicodeInfo.GetUnicodeCategory(c) })
                .Where(@t => @t.uc != UnicodeCategory.NonSpacingMark)
                .Select(@t => @t.c);
        var cleanStr = new string([.. chars]).Normalize(NormalizationForm.FormC);
        return cleanStr;
    }

    [GeneratedRegex("[^a-zA-Z0-9]+")]
    private static partial Regex DNSRegEx();
}
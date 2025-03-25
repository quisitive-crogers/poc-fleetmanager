namespace System;

/// <summary>
/// Boolean extensions static class. 
/// </summary>
public static class BooleanExtensions
{
    /// <summary>
    /// Converts to sqlbit.
    /// </summary>
    /// <param name="boolean">if set to <c>true</c> [boolean].</param>
    /// <returns></returns>
    public static int ToSqlBit(this bool boolean)
    {
        return boolean ? 1 : 0;
    }

    /// <summary>
    /// Converts to Yes/No String.
    /// </summary>
    /// <param name="value">if set to <c>true</c> [value].</param>
    /// <returns></returns>
    public static string ToYesNoString(this bool value)
    {
        return value ? "Yes" : "No";
    }

    /// <summary>
    /// Converts to Yes/No String.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="nullValue">The null value.</param>
    /// <returns></returns>
    public static string ToYesNoString(this bool? value, string nullValue = "")
    {
        if (!value.HasValue) return nullValue;
        return value.Value ? "Yes" : "No";
    }
}
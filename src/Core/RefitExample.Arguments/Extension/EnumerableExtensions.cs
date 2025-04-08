namespace RefitExample.Arguments.Extension;

public static class EnumerableExtensions
{
    public static bool In<T>(this T value, params T[] values) => values.Contains(value);
}
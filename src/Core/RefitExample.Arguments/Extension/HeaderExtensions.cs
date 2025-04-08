using Microsoft.AspNetCore.Http;

namespace RefitExample.Arguments.Extension;

public static class HeaderExtensions
{
    public static T? GetHeaderValue<T>(this HttpRequestMessage request, string headerName, TryParseDelegate<T> parser)
    {
        if (request.Headers.TryGetValues(headerName, out var values))
        {
            var value = values.FirstOrDefault();
            if (value is not null && parser(value, out var result))
                return result;
        }

        return default;
    }

    public static T? GetHeaderValue<T>(this IHeaderDictionary headers, string headerName, TryParseDelegate<T> parser)
    {
        var value = headers[headerName].FirstOrDefault();
        if (value is not null && parser(value, out var result))
            return result;

        return default;
    }

    public delegate bool TryParseDelegate<T>(string input, out T result);
}
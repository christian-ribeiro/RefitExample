using System.Net.Http.Headers;

namespace RefitMicroserviceAuth.Arguments.Extension;

public static class HttpRequestMessageExtensions
{
    public static void SetBearerToken(this HttpRequestMessage request, string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
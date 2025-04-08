using Microsoft.AspNetCore.Http;
using RefitMicroserviceAuth.Arguments.Argument.Session;
using RefitMicroserviceAuth.Arguments.Const;

namespace RefitMicroserviceAuth.Arguments.Extension;

public static class HttpContextExtensions
{
    public static Guid GetGuidSessionDataRequest(this HttpContext context)
    {
        return context.Request.Headers.GetHeaderValue<Guid>(ConfigurationConst.GuidSessionDataRequest, Guid.TryParse);
    }

    public static long GetLoggedEnterprise(this HttpContext context)
    {
        var guid = context.GetGuidSessionDataRequest();
        return SessionData.GetLoggedEnterprise(guid) ?? 0;
    }
}
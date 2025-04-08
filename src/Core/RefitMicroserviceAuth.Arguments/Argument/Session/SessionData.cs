using System.Collections.Concurrent;

namespace RefitMicroserviceAuth.Arguments.Argument.Session
{
    public static class SessionData
    {
        private static readonly ConcurrentDictionary<Guid, SessionDataRequest> _listSessionDataRequest = new();

        public static Guid Initialize() => Add(new SessionDataRequest());

        public static Guid Add(SessionDataRequest sessionDataRequest)
        {
            _listSessionDataRequest.TryAdd(sessionDataRequest.GuidSessionDataRequest, sessionDataRequest);
            return sessionDataRequest.GuidSessionDataRequest;
        }

        public static void SetLoggedEnterprise(Guid guidSessionDataRequest, long enterpriseId)
        {
            if (_listSessionDataRequest.TryGetValue(guidSessionDataRequest, out var sessionData))
                sessionData.EnterpriseId = enterpriseId;
        }

        public static long? GetLoggedEnterprise(Guid guidSessionDataRequest)
        {
            return _listSessionDataRequest.TryGetValue(guidSessionDataRequest, out var sessionData) ? sessionData.EnterpriseId : null;
        }
    }
}
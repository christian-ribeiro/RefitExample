using RefitExample.Arguments.Enum.Microservice;
using System.Collections.Concurrent;

namespace RefitExample.Arguments.Argument.Session
{
    public static class SessionData
    {
        private static readonly ConcurrentDictionary<Guid, SessionDataRequest> _listSessionDataRequest = new();
        private static readonly List<MicroserviceAuthentication> ListMicroserviceAuthentication = [];

        public static Guid Initialize() => Add(new SessionDataRequest());

        public static Guid Add(SessionDataRequest sessionDataRequest)
        {
            _listSessionDataRequest.TryAdd(sessionDataRequest.GuidSessionDataRequest, sessionDataRequest);
            return sessionDataRequest.GuidSessionDataRequest;
        }

        public static void SetMicroserviceAuthentication(MicroserviceAuthentication microserviceAuthentication)
        {
            ListMicroserviceAuthentication.Add(microserviceAuthentication);
        }

        public static void SetLoggedEnterprise(Guid guidSessionDataRequest, long enterpriseId)
        {
            if (_listSessionDataRequest.TryGetValue(guidSessionDataRequest, out var sessionData))
                sessionData.EnterpriseId = enterpriseId;
        }

        public static MicroserviceAuthentication? GetMicroserviceAuthentication(EnumMicroservice microservice, long enterpriseId)
        {
            return ListMicroserviceAuthentication.Where(x => x.Microservice == microservice && x.EntepriseId == enterpriseId).FirstOrDefault();
        }

        public static long? GetLoggedEnterprise(Guid guidSessionDataRequest)
        {
            return _listSessionDataRequest.TryGetValue(guidSessionDataRequest, out var sessionData) ? sessionData.EnterpriseId : null;
        }
    }
}
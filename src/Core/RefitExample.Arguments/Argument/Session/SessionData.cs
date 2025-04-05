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

        public static void AddMicroserviceAuthentication(MicroserviceAuthentication microserviceAuthentication) =>
                ListMicroserviceAuthentication.Add(microserviceAuthentication);

        public static MicroserviceAuthentication? GetMicroserviceAuthentication(EnumMicroservice microservice, long enterpriseId) =>
            ListMicroserviceAuthentication.Where(x => x.Microservice == microservice && x.EntepriseId == enterpriseId).FirstOrDefault();
    }
}
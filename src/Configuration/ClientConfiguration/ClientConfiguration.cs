using System.Collections.Generic;
using MessageBrokerTester.BrokerClients;

namespace MessageBrokerTester.Configuration
{
    public abstract class ClientConfiguration
    {
        public ClientType ClientType = ClientType.ProbabilityClient;
        public List<string> TopicList = new List<string>();
        public int TestDurationInMinutes = 1;
    }
}
using MessageBrokerTester.Broker;

namespace MessageBrokerTester.Configuration
{
    public abstract class BrokerConfiguration
    {
        public BrokerType BrokerType = BrokerType.Mqtt;
        public string BrokerEndpoint = "127.0.0.1";
    }
}
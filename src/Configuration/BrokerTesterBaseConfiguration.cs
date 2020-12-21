namespace MessageBrokerTester.Configuration
{
    public class BrokerTesterBaseConfiguration
    {
        public int ClientCount = 10;
        public BrokerConfiguration BrokerConfiguration = new MqttBrokerConfiguration();
        public ClientConfiguration ClientConfiguration = new ProbabilityClientConfiguration();
        public MessageBuilderConfiguration MessageBuilderConfiguration = new MessageBuilderConfiguration();
    }
}
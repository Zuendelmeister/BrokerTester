using System;
using MessageBrokerTester.Broker;
using MessageBrokerTester.Configuration;

namespace MessageBrokerTester.Factories
{
    public class BrokerFactory
    {
        private readonly BrokerConfiguration brokerConfiguration;
        public BrokerFactory(BrokerConfiguration brokerConfiguration)
        {
            this.brokerConfiguration = brokerConfiguration;
        }
        
        public IBroker Create(string clientName)
        {
            switch (this.brokerConfiguration.BrokerType)
            {
                case BrokerType.Mqtt:
                    MqttBroker broker = new MqttBroker((MqttBrokerConfiguration)this.brokerConfiguration, clientName);
                    return broker;
                default:
                    throw new Exception($"{this.brokerConfiguration.BrokerType} is not supported yet");
            }
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using MessageBrokerTester.Broker;
using MessageBrokerTester.BrokerClients;
using MessageBrokerTester.Configuration;
using MessageBrokerTester.MessageBuilder;
using Newtonsoft.Json.Linq;

namespace MessageBrokerTester.Parsers
{
    public static class ConfigurationParser
    { 
        private const string BrokerConfigurationProperty = "BrokerConfiguration";
        private const string MessageBuilderConfigurationProperty = "MessageBuilderConfiguration";
        private const string ClientConfigurationProperty = "ClientConfiguration";

        private const string BrokerTypeProperty = "BrokerType";
        private const string MessageBuilderTypeProperty = "MessageBuilderType";
        private const string ClientTypeProperty = "ClientType";
        
        public static async Task<BrokerTesterBaseConfiguration> GetConfiguration(string configFilePath)
        {
            string configurationString = await File.ReadAllTextAsync(configFilePath); 
            JObject job = JObject.Parse(configurationString);

            if (!job.ContainsKey(BrokerConfigurationProperty))
                throw new Exception($"{BrokerConfigurationProperty} not found in configuration.");
            if (!job.ContainsKey(MessageBuilderConfigurationProperty))
                throw new Exception($"{MessageBuilderConfigurationProperty} not found in configuration.");
            if (!job.ContainsKey(ClientConfigurationProperty))
                throw new Exception($"{ClientConfigurationProperty} not found in configuration.");
            
            BrokerTesterBaseConfiguration configuration = job.ToObject<BrokerTesterBaseConfiguration>();
            configuration.BrokerConfiguration = GetBrokerConfiguration((JObject)job[BrokerConfigurationProperty]);
            configuration.MessageBuilderConfiguration = GetMessageBuilderConfiguration((JObject)job[MessageBuilderConfigurationProperty]);
            configuration.ClientConfiguration = GetClientConfiguration((JObject)job[ClientConfigurationProperty]);
            
            return configuration;
        }
        private static BrokerConfiguration GetBrokerConfiguration(JObject jsonConfiguration)
        {
            if(!jsonConfiguration.ContainsKey(BrokerTypeProperty))
            {
                throw new Exception("ClientType is missing in Configuration");
            }
            string brokerType = (string) jsonConfiguration[BrokerTypeProperty];
            Enum.TryParse(brokerType, out BrokerType type);
            switch (type)
            {
                case BrokerType.Mqtt:
                    return jsonConfiguration.ToObject<MqttBrokerConfiguration>();
                default:
                    throw new Exception($"{brokerType} is not supported yet");
            }
        }
        
        private static ClientConfiguration GetClientConfiguration(JObject jsonConfiguration)
        {
            if(!jsonConfiguration.ContainsKey(ClientTypeProperty))
            {
                throw new Exception("ClientType is missing in Configuration");
            }
            string clientType = (string) jsonConfiguration[ClientTypeProperty];
            Enum.TryParse(clientType, out ClientType type);
            switch (type)
            {
                case ClientType.ProbabilityClient:
                    return jsonConfiguration.ToObject<ProbabilityClientConfiguration>();
                default:
                    throw new Exception($"{clientType} is not supported yet");
            }
        }
        
        private static MessageBuilderConfiguration GetMessageBuilderConfiguration(JObject jsonConfiguration)
        {
            if(!jsonConfiguration.ContainsKey(MessageBuilderTypeProperty))
            {
                throw new Exception("ClientType is missing in Configuration");
            }
            string messageBuilderType = (string) jsonConfiguration[MessageBuilderTypeProperty];
            Enum.TryParse(messageBuilderType, out MessageBuilderType type);
            switch (type)
            {
                case MessageBuilderType.SinusValueMessageBuilder:
                    return jsonConfiguration.ToObject<SinusValueMessageBuilderConfiguration>();
                case MessageBuilderType.RandomIntArrayMessageBuilder:
                    return jsonConfiguration.ToObject<RandomIntArrayMessageBuilderConfiguration>();
                default:
                    throw new Exception($"{messageBuilderType} is not supported yet");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBrokerTester.Configuration;
using MessageBrokerTester.Parsers;
using Newtonsoft.Json;

namespace MessageBrokerTester.Factories
{
    public class MotherFactory
    {
        private readonly string configFilePath;
        private BrokerTesterBaseConfiguration configuration;
        public MotherFactory(string configFilePath = "/App/configuration.json")
        {
            this.configFilePath = configFilePath;
        }

        public async Task BuildAndRunClients()
        {
            this.configuration = await ConfigurationParser.GetConfiguration(this.configFilePath);
            Console.WriteLine($"Working with the following Config: {JsonConvert.SerializeObject(this.configuration)}");
            Console.WriteLine(
                $"Creating {this.configuration.ClientCount} clients.");
            Console.WriteLine($"Test will take {this.configuration.ClientConfiguration.TestDurationInMinutes} minutes.");
            BrokerFactory brokerFactory = new BrokerFactory(this.configuration.BrokerConfiguration);
            MessageBuilderFactory messageBuilderFactory = new MessageBuilderFactory(this.configuration.MessageBuilderConfiguration);
            ClientFactory clientFactory = new ClientFactory(this.configuration.ClientConfiguration,brokerFactory,messageBuilderFactory);
            var clients = clientFactory.Create(this.configuration.ClientCount);
            
            List<Task> taskList = new List<Task>();
            foreach (var client in clients)
            {
                var clientTask = client.RunAsync();
                taskList.Add(clientTask);
            }
            try
            {
                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException  e)
            {
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", e.InnerExceptions[j].ToString());
                }
            }
            Console.WriteLine($"Test ended.");
        }
    }
}
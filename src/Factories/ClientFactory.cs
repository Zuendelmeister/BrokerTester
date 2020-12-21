using System;
using System.Collections.Generic;
using MessageBrokerTester.BrokerClients;
using MessageBrokerTester.Configuration;

namespace MessageBrokerTester.Factories
{
    public class ClientFactory
    {
        private readonly ClientConfiguration clientConfiguration;
        private readonly BrokerFactory brokerFactory;
        private readonly MessageBuilderFactory messageBuilderFactory;
        public ClientFactory(ClientConfiguration clientConfiguration,BrokerFactory brokerFactory, MessageBuilderFactory messageBuilderFactory)
        {
            this.brokerFactory = brokerFactory;
            this.messageBuilderFactory = messageBuilderFactory;
            this.clientConfiguration = clientConfiguration;
        }

        public List<Client> Create(int clientCount)
        {
            var clientList = new List<Client>();
            switch (this.clientConfiguration.ClientType)
            {
                case ClientType.ProbabilityClient:
                    
                    for (int i = 0; i <= clientCount; ++i)
                    {
                        string clientName = $"client-{i}";
                        ProbabilityClient client = new ProbabilityClient(clientName,(ProbabilityClientConfiguration)clientConfiguration);
                        client.SetBroker(this.brokerFactory.Create(clientName));
                        client.SetMessageBuilder(this.messageBuilderFactory.Create());
                        clientList.Add(client);
                    }
                    break;
                default:
                    throw new Exception("Could not create Clients");
            }
            
            return clientList;
        }
    }
}
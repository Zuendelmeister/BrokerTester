using System;
using System.Threading.Tasks;
using MessageBrokerTester.Broker;
using MessageBrokerTester.MessageBuilder;

namespace MessageBrokerTester.BrokerClients
{
    public abstract class Client : IDisposable
    {
        protected IMessageBuilder messageBuilder;
        protected readonly string clientName;
        protected IBroker broker;
        public Client(string clientName)
        {
            this.clientName = clientName;
        }

        public void SetMessageBuilder(IMessageBuilder messageBuilder)
        {
            this.messageBuilder = messageBuilder;
        }
        public void SetBroker(IBroker broker)
        {
            this.broker = broker;
        }
        public abstract Task RunAsync();
        public void Dispose()
        {
            this.messageBuilder.Dispose();
        }
    }
}
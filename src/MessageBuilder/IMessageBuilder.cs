using System;

namespace MessageBrokerTester.MessageBuilder
{
    public interface IMessageBuilder : IDisposable
    {
        string GetMessage(string clientName);
    }
}
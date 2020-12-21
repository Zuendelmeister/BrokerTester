using System;
using System.Threading.Tasks;

namespace MessageBrokerTester.Broker
{
    public interface IBroker:IDisposable
    {
        Task<bool> ConnectAsync();
        Task<bool>  DisconnectAsync();
        Task<bool> PublishAsync(string topic, string message);
        Task<bool> SubscribeToTopic(string topic);
        Task<bool> UnsubscribeFromTopic(string topic);
        bool IsConnected();
        void UseMessageReceivedAction(Func<string,string>  handler);
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBrokerTester.Broker;

namespace MessageBrokerTester.MessageStatistics
{
    /// <summary>
    /// Yet to come
    /// </summary>
    public class BrokerMessageStatistiks
    {
        private IBroker broker;
        public  ConcurrentDictionary<string,List<DateTime>> MessageReceivedPerTopic = new ConcurrentDictionary<string, List<DateTime>>();
        public  ConcurrentDictionary<string,List<double>> MessageReceivedTimeDiffPerTopic = new ConcurrentDictionary<string, List<double>>();
        private DateTime lastReceived;
        public delegate void Del(string message);

        public BrokerMessageStatistiks(IBroker broker)
        {
            this.broker = broker;
            this.broker.UseMessageReceivedAction(DelegateMethod);
        }

        public async Task Subscribe(List<string> topicList)
        {
            if (this.broker.IsConnected())
            {
                foreach (var topic in topicList)
                {
                    await this.broker.SubscribeToTopic(topic);
                }    
            }
        }
        
        public string DelegateMethod(string topic)
        {
            if (!MessageReceivedPerTopic.ContainsKey(topic))
            {
                this.MessageReceivedPerTopic.TryAdd(topic,new List<DateTime>{DateTime.Now});
                this.MessageReceivedTimeDiffPerTopic.TryAdd(topic,new List<double>());
            }
            else
            {
                this.MessageReceivedPerTopic[topic].Add(DateTime.Now);
                TimeSpan span = this.lastReceived - DateTime.Now;
                this.MessageReceivedTimeDiffPerTopic[topic].Add(span.TotalMilliseconds);
            }
            this.lastReceived = DateTime.Now;

            return "";
        }
    }
}
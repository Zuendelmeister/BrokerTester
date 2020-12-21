using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBrokerTester.Configuration;

namespace MessageBrokerTester.BrokerClients
{
    public class ProbabilityClient : Client
    {
        private ProbabilityClientConfiguration configuration;
        private static readonly Random _random = new Random();
        private List<string> ActiveSubscriptionList = new List<string>();
        
        public ProbabilityClient(string clientName, ProbabilityClientConfiguration configuration) : base(clientName)
        {

            this.configuration = configuration;
            if (this.configuration.TopicList.Count < 1)
            {
                throw new Exception("No topics to work with");
            }
        }
        
        public override async Task RunAsync()
        {
            DateTime startTime = DateTime.Now;
            
            try
            {
                while (DateTime.Now - startTime < TimeSpan.FromMinutes(this.configuration.TestDurationInMinutes))
                {
                    await ConnectIfProbable();
                    if (this.broker.IsConnected())
                    {
                        await PublishIfProbable();
                        await SubscribeIfProbable();
                        await UnsubscribeIfProbable();
                        await DisconnectIfProbable();
                    }
                    await Task.Delay(this.configuration.TickMs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task ConnectIfProbable()
        {
            if (!this.broker.IsConnected() && CheckProbability(this.configuration.ConnectProbability))
            {
                await this.broker.ConnectAsync();
            }
        }

        private async Task DisconnectIfProbable()
        {
            if (CheckProbability(this.configuration.DiscConnectProbability))
            {
                await this.broker.DisconnectAsync();
            }
        }

        private async Task PublishIfProbable()
        {
            try
            {
                if (CheckProbability(this.configuration.PublishProbability))
                {
                    await this.broker.PublishAsync(
                        this.configuration.TopicList[_random.Next(this.configuration.TopicList.Count)],
                        this.messageBuilder.GetMessage(clientName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private async Task SubscribeIfProbable()
        {
            if (CheckProbability(this.configuration.AddSubscriptionProbability))
            {
                string topic = GetUnusedSubspriptionTopic();
                if (topic != "")
                {
                    await this.broker.SubscribeToTopic(topic);
                }
            }
        }

        private async Task UnsubscribeIfProbable()
        {
            if (CheckProbability(this.configuration.RemoveSubscriptionProbability))
            {
                if (ActiveSubscriptionList.Count > 0)
                {
                    await this.broker.UnsubscribeFromTopic(ActiveSubscriptionList[0]);
                    this.ActiveSubscriptionList.RemoveAt(0);
                }
            }
        }
        private string GetUnusedSubspriptionTopic()
        {
            foreach (string topic in this.configuration.TopicList)
            {
                if (!ActiveSubscriptionList.Contains(topic))
                {
                    ActiveSubscriptionList.Add(topic);
                    return topic;
                }
            }
            return "";
        }

        private bool CheckProbability(int probability)
        {
            int perCent = _random.Next(0, 1000);
            if (perCent < probability)
            {
                return true;
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using MessageBrokerTester.Configuration;
using Newtonsoft.Json;

namespace MessageBrokerTester.MessageBuilder
{
    public class RandomIntArrayMessageBuilder : IMessageBuilder
    {
        private readonly RandomIntArrayMessageBuilderConfiguration configuration;
        private static readonly Random _random = new Random();
        
        public RandomIntArrayMessageBuilder(RandomIntArrayMessageBuilderConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetMessage(string clientName)
        {
            var valueList = new List<int>();
            for (var i = 0; i<= this.configuration.ElementCount; ++i)
            {
                valueList.Add(_random.Next(this.configuration.MinValue, this.configuration.MaxValue));    
            }
            
            var messageObject = new { timestamp = DateTime.Now, IntArray = valueList, clientName = clientName };
            return JsonConvert.SerializeObject(messageObject);
        }

        public void Dispose()
        {
        }
    }
}
using System;
using MessageBrokerTester.Configuration;
using Newtonsoft.Json;

namespace MessageBrokerTester.MessageBuilder
{
    public class SinusValueMessageBuilder : IMessageBuilder
    {
        private static readonly Random _random = new Random();
        private int lastAngle = 0;
        private readonly SinusValueMessageBuilderConfiguration configuration;
        
        public SinusValueMessageBuilder(SinusValueMessageBuilderConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetMessage(string clientName)
        {
            
            var sinVal=Math.Sin( (this.lastAngle * (Math.PI)) / 180 );
            
            var messageObject = new { timestamp = DateTime.Now, Value = sinVal * this.configuration.Amplitude, clientName = clientName };
            this.lastAngle +=this.configuration.Increment;
            if (this.lastAngle > 360)
            {
                this.lastAngle -= 360;
            }
            return JsonConvert.SerializeObject(messageObject);
        }

        public void Dispose()
        {
        }
    }
}
using System;
using MessageBrokerTester.Configuration;
using MessageBrokerTester.MessageBuilder;

namespace MessageBrokerTester.Factories
{
    public class MessageBuilderFactory
    {
        private readonly MessageBuilderConfiguration messageBuilderConfiguration;
        public MessageBuilderFactory(MessageBuilderConfiguration messageBuilderConfiguration)
        {
            this.messageBuilderConfiguration = messageBuilderConfiguration;
        }
       
        public IMessageBuilder Create()
        {
            switch (this.messageBuilderConfiguration.MessageBuilderType)
            {
                case MessageBuilderType.SinusValueMessageBuilder:
                    SinusValueMessageBuilder sinusMessageBuilder = new SinusValueMessageBuilder((SinusValueMessageBuilderConfiguration)this.messageBuilderConfiguration);
                    return sinusMessageBuilder;
                case MessageBuilderType.RandomIntArrayMessageBuilder:
                    
                    RandomIntArrayMessageBuilder randomIntArrayMessageBuilder = new RandomIntArrayMessageBuilder((RandomIntArrayMessageBuilderConfiguration)this.messageBuilderConfiguration);
                    return randomIntArrayMessageBuilder;
                default:
                    throw new Exception($"{this.messageBuilderConfiguration.MessageBuilderType} is not supported yet");
            }
        }
    }
}
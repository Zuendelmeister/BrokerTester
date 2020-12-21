namespace MessageBrokerTester.Configuration
{
    public class RandomIntArrayMessageBuilderConfiguration:MessageBuilderConfiguration
    {
        public int ElementCount = 10;
        public int MinValue = -10000;
        public int MaxValue = 10000;
    }
}
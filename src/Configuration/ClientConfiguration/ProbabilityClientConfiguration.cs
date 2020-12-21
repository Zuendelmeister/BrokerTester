namespace MessageBrokerTester.Configuration
{
    /// <summary>
    /// Uses probabilities to 1000 to determine whether to do something or not
    /// </summary>
    public class ProbabilityClientConfiguration : ClientConfiguration
    {
        public readonly int ConnectProbability = 100;
        public readonly int DiscConnectProbability = 100;
        public readonly int AddSubscriptionProbability = 100;
        public readonly int RemoveSubscriptionProbability = 100;
        public readonly int PublishProbability = 100;
        public readonly int TickMs = 1000;

    }
}
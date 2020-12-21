using System;
using MessageBrokerTester.Factories;

namespace MessageBrokerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var factory = new MotherFactory(@"C:\Work\gits\priv\MessageBrokerTester\configuration\configuration_Kafka-Probability-IntArray.json");
                var factory = new MotherFactory();
                factory.BuildAndRunClients().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MessageBrokerTester.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MessageBrokerTester.Broker
{
    public class MqttBroker : IBroker
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;
        private readonly MqttBrokerConfiguration configuration;
        private bool plannedDisconnect = false;
        private readonly string clientId; 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brokerEndpoint">Broker IP or Hostname</param>
        /// /// <param name="brokerPort">Connection port of the broker</param>
        public MqttBroker(MqttBrokerConfiguration configuration, string clientId)
        {
            this.clientId = clientId;
            this.configuration = configuration;
            var factory = new MqttFactory();
            this.options = new MqttClientOptionsBuilder()
                .WithTcpServer(this.configuration.BrokerEndpoint, this.configuration.BrokerPort)
                .WithClientId(this.clientId)
                .Build();
            this.mqttClient = factory.CreateMqttClient();
            
            
            this.mqttClient.UseDisconnectedHandler(async e =>
            {
                if (!this.plannedDisconnect)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await ConnectAsync();
                }
                else
                {
                    this.plannedDisconnect = false;
                }
            });
        }
        public async Task<bool> ConnectAsync()
        {
            try
            {
                Console.WriteLine($"{this.clientId} connecting to {this.configuration.BrokerEndpoint} on port {this.configuration.BrokerPort}");
                await this.mqttClient.ConnectAsync(options, CancellationToken.None); //Real CancellationTokens will be added
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{this.clientId} CONNECTING FAILED");
            }
            return true;
        }

        public async Task<bool> DisconnectAsync()
        {
            this.plannedDisconnect = true;
            Console.WriteLine($"{this.clientId} disconnecting to {this.configuration.BrokerEndpoint} on port {this.configuration.BrokerPort}");
            await this.mqttClient.DisconnectAsync();
            return true;
        }

        public async Task<bool> PublishAsync(string topic="test", string message="test")
        {
            try
            {
                Console.WriteLine($"{this.clientId} sending {message} to topic {topic}");
                await this.mqttClient.PublishAsync(topic,message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return true;
        }

        public async Task<bool> SubscribeToTopic(string topic)
        {
            Console.WriteLine($"{this.clientId} subscribing to {topic}");
            await this.mqttClient.SubscribeAsync(topic);
            return true;
        }

        public async Task<bool> UnsubscribeFromTopic(string topic)
        {
            Console.WriteLine($"{this.clientId} unsubscribing to {topic}");
            await this.mqttClient.UnsubscribeAsync(topic);
            return true;
        }
        
        public async void Dispose()
        {
            await this.mqttClient?.DisconnectAsync();
            this.mqttClient?.Dispose();
        }

        public void UseMessageReceivedAction(Func<string,string> handler)
        {
            this.mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                handler(e.ApplicationMessage.Topic);
            });
        }
           
        public bool IsConnected()
        {
            return this.mqttClient.IsConnected;
        }
    }
}
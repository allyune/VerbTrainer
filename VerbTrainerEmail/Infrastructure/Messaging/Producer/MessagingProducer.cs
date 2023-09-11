using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;

namespace VerbTrainer.Infrastructure.Messaging.Producer
{
    public class MessagingProducer : IMessagingProducer
    {
        private readonly IRabbitMqConnectionFactory _factoryBuilder;
        private readonly IConfiguration _configuration;

        public MessagingProducer(IRabbitMqConnectionFactory factory, IConfiguration configuration)
        {
            _factoryBuilder = factory;
            _configuration = configuration;
        }

        public void SendMessage<T>(T message, string rountingKey)
        {
            var exchangeConfig = _configuration.GetSection("MessagingExchangeSettings");
            var factory = _factoryBuilder.CreateConnectionFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: exchangeConfig["Name"],
                type: exchangeConfig["Type"]);

            foreach (var queueSettings in exchangeConfig.GetSection("Queues").GetChildren())
            {
                channel.QueueDeclare(
                    queue: queueSettings["Name"]
                );
            }

            Console.WriteLine(message);
            var json = JsonConvert.SerializeObject(message);
            Console.WriteLine(json);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: exchangeConfig["Name"], routingKey: rountingKey, body: body);

        }
    }
}
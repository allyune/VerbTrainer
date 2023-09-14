using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using VerbTrainerUser.Infrastructure.Messaging.Configuration;
using VerbTrainerMessaging;

namespace VerbTrainerUser.Infrastructure.Messaging.Producer
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
            var factory = _factoryBuilder.CreateConnectionFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string centralExchangeName = CentralExchange.SetupExchange(channel);


            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: centralExchangeName, routingKey: rountingKey, body: body);

        }
    }
}
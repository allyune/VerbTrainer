using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;
using VerbTrainerMessaging;
using RabbitMQ.Client.Events;
using VerbTrainerEmail.Infrastructure.Messaging.Consumer;
using VerbTrainerEmail.Application.SendPasswordResetEmail.Handler;

namespace VerbTrainer.Infrastructure.Messaging.Consumer
{
    public class MessagingConsumer : IMessagingConsumer
    {
        private readonly IRabbitMqConnectionFactory _factoryBuilder;
        private readonly IConfiguration _configuration;
        private readonly IPasswordResetEmailHandler _resetEmail;

        public MessagingConsumer(
            IRabbitMqConnectionFactory factory,
            IConfiguration configuration,
            IPasswordResetEmailHandler resetEmail)
        {
            _factoryBuilder = factory;
            _configuration = configuration;
            _resetEmail = resetEmail;
        }

        public async Task StartConsumingMessages()
        {
            Console.WriteLine("LISTENING TO MESSAGES");
            var factory = _factoryBuilder.CreateConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string exchangeName = CentralExchange.SetupExchange(channel);

            foreach (var queueSettings in _configuration
                                          .GetSection("Queues")
                                          .GetChildren())
            {
                var queueName = queueSettings["Name"];
                var routingKeys = queueSettings.GetSection("RoutingKeys").Get<string[]>();

                channel.QueueDeclare(
                    queue: queueName,
                    durable: bool.Parse(queueSettings["Durable"]),
                    exclusive: bool.Parse(queueSettings["Exclusive"]),
                    autoDelete: bool.Parse(queueSettings["AutoDelete"])
                );

                foreach (var routingKey in routingKeys)
                {
                    channel.QueueBind(
                        queue: queueName,
                        exchange: exchangeName,
                        routingKey: routingKey
                    );
                }
            }

            var consumer = new EventingBasicConsumer(channel);

            //function should be async
            consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Message received: {message}");
                    await _resetEmail.SendPasswordResetEmail(message);
                    channel.BasicAck(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Exception occurred while processing message: {ex}");
                }
            };

            channel.BasicConsume(
                queue: "password_reminder_queue",
                autoAck: true,
                consumer: consumer);
        }

    }
}
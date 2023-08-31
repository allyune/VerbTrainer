using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;

namespace VerbTrainerEmail.Infrastructure.Messaging.Configuration
{

    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public RabbitMqConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ConnectionFactory CreateConnectionFactory()
        {
            var connectionConfig = _configuration.GetSection("MessagingSettings");

            var factory = new ConnectionFactory
            {
                HostName = connectionConfig["HostName"],
                Port = connectionConfig.GetValue<int>("Port"),
                UserName = connectionConfig["UserName"],
                Password = connectionConfig["Password"],
                ClientProvidedName = connectionConfig["ClientProvidedName"]
            };

            return factory;
        }
    }
}
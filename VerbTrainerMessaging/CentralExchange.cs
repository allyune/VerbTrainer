using System;
using RabbitMQ.Client;

namespace VerbTrainerMessaging
{
	public static class CentralExchange
	{
        public static string SetupExchange(IModel channel)
        {
            string exchangeName = "central_exchange";
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct
            );

            return exchangeName;
        }
    }
}


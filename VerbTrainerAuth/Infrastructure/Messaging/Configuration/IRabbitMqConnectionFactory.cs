using System;
using RabbitMQ.Client;

namespace VerbTrainerAuth.Infrastructure.Messaging.Configuration
{
	public interface IRabbitMqConnectionFactory
	{
		public ConnectionFactory CreateConnectionFactory();
    }
}


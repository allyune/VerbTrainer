using System;
using RabbitMQ.Client;

namespace VerbTrainerEmail.Infrastructure.Messaging.Configuration
{
	public interface IRabbitMqConnectionFactory
	{
		public ConnectionFactory CreateConnectionFactory();
    }
}


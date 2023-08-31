using System;
using RabbitMQ.Client;

namespace VerbTrainer.Infrastructure.Messaging.Configuration
{
	public interface IRabbitMqConnectionFactory
	{
		public ConnectionFactory CreateConnectionFactory();
    }
}


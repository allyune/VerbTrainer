using System;
using RabbitMQ.Client;

namespace VerbTrainerUser.Infrastructure.Messaging.Configuration
{
	public interface IRabbitMqConnectionFactory
	{
		public ConnectionFactory CreateConnectionFactory();
    }
}


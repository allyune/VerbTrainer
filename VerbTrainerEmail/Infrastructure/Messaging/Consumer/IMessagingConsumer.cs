using System;

namespace VerbTrainerEmail.Infrastructure.Messaging.Consumer
{
	public interface IMessagingConsumer
	{
		public Task StartConsumingMessages();

    }
}


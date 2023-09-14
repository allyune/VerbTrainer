using System;
namespace VerbTrainerUser.Infrastructure.Messaging.Producer
{
	public interface IMessagingProducer
	{
        public void SendMessage<T>(T message, string rountingKey);
    }
}


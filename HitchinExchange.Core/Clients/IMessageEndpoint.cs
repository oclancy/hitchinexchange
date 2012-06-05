using System;
namespace HitchinExchange.Core.Clients
{
    public interface IMessageEndpoint : IDisposable
    {
        event MessageEndpoint.MessageHandler MessageReceived;

        void Publish(QuickFix.Message msg);
        void RegisterPublishType(Type type, string routingKey);
        void Subscribe(string queueName, string key);
    }
}

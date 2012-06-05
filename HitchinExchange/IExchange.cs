using System;
namespace HitchinExchange
{
    public interface IExchange : IDisposable
    {
        HitchinExchange.Core.FixAcceptor Endpoint { get; set; }
        HitchinExchange.Core.Clients.IMessageEndpoint MqEndpoint { get; set; }
    }
}

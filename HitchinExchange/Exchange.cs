using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using QuickFix;
using System.Reflection;
using HitchinExchange.Core;
using HitchinExchange.Core.Clients;

namespace HitchinExchange
{
    public class Exchange : IExchange
    {
        private bool m_disposed = false;
        private string m_appName;

        RabbitMQ.Client.IConnection m_mqClientConn;
        RabbitMQ.Client.IModel m_mqModel;

        public FixAcceptor Endpoint { get; set; }

        public IMessageEndpoint MqEndpoint { get; set; }

        public Exchange() 
        {
            m_appName = Assembly.GetEntryAssembly().GetName().Name;

            CreateFixEndpoint();

            CreateMqSubscriber();
        }

        private void CreateMqSubscriber()
        {
            MqEndpoint = new MessageEndpoint("Hitchin");

            MqEndpoint.MessageReceived += MqEndpoint_MessageReceived;

            MqEndpoint.RegisterPublishType(typeof(QuickFix42.ExecutionReport), "Messages.ExecutionReport.42");

            MqEndpoint.RegisterPublishType(typeof(QuickFix42.Quote), "Messages.Quote.42");

            MqEndpoint.Subscribe("Exchange", "Messages.NewOrderSingle.*");

            MqEndpoint.Subscribe("Exchange", "Message.ExecutionReport.*");
        }

        void MqEndpoint_MessageReceived( Message e)
        {
            Console.WriteLine(e.ToString());

            Endpoint.SendMessage(e);
        }

        private void CreateFixEndpoint()
        {
            Endpoint = new FixAcceptor(@"Exchange.cfg",
                                            "Exchange",
                                             OnMessage);

            Endpoint.Start();
        }

        void OnMessage(Message message, SessionID sessionId)
        {
            MqEndpoint.Publish(message);
            return;

            var props = m_mqModel.CreateBasicProperties();
            props.Headers = new Dictionary<object, object>();
            props.Headers.Add("QFSessionId", sessionId.ToString());
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool isDisposing)
        {
            if (!m_disposed)
            {
                if (isDisposing)
                {
                    // Free other state (managed objects).
                    m_mqModel.Close();
                    m_mqClientConn.Close();
                    Endpoint.Stop();
                }

                // Set large fields to null.
                m_disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Exchange()
        {
            Dispose(false);
        } 
        #endregion
    }
}

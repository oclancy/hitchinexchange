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
    public class Exchange : IDisposable
    {
        private bool m_disposed = false;
        private string m_appName;

        RabbitMQ.Client.IConnection m_mqClientConn;
        RabbitMQ.Client.IModel m_mqModel;

        public FixAcceptor Endpoint { get; set; }

        public MessageEndpoint MsgEndpoint { get; set; }

        public Exchange() 
        {
            m_appName = Assembly.GetEntryAssembly().GetName().Name;
            var cf = new ConnectionFactory();

            //m_mqClientConn = cf.CreateConnection();

            //m_mqModel = m_mqClientConn.CreateModel();

            //m_mqModel.ExchangeDeclare("Hitchin", ExchangeType.Topic);

            //m_mqModel.QueueDeclare("Exchange", true, false, false, null);

            //m_mqModel.QueueBind("Exchange", "Hitchin", string.Empty);

            CreateFixEndpoint();

            CreateMqSubscriber();
        }

        private void CreateMqSubscriber()
        {
            MsgEndpoint = new MessageEndpoint("Hitchin");

            MsgEndpoint.MessageReceived += MsgEndpoint_MessageReceived;

            MsgEndpoint.RegisterPublishType(typeof(QuickFix42.ExecutionReport), "Messages.ExecutionReport.42");

            MsgEndpoint.Subscribe("Exchange", "Messages.NewOrderSingle.*");

            //MsgEndpoint.Subscribe("Exchange", "Message.ExecutionReport.*");
        }

        void MsgEndpoint_MessageReceived( Message e)
        {
            Console.WriteLine(e.ToString());

            //if (e.getField(QuickFix.MsgType.FIELD) == "D")
            //{
                Session.sendToTarget(e);
            //}
        }

        private void CreateFixEndpoint()
        {
            Endpoint = new FixAcceptor(@"Exchange.cfg",
                                            "Exchange",
                                             OnMessage);
        }

        void OnMessage(Message message, SessionID sessionId)
        {
            var props = m_mqModel.CreateBasicProperties();
            props.Headers = new Dictionary<object, object>();
            props.Headers.Add("QFSessionId", sessionId.ToString());


        }

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
    }
}

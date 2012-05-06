using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System.Threading.Tasks;
using QuickFix;
using System.Threading;

namespace HitchinExchange.Core.Clients
{
    public class MessageEndpoint : IDisposable
    {
        public delegate void MessageHandler(Message msg);

        Dictionary<string, Subscription> m_subsDict;
        Dictionary<Type, string> m_typeQueueDict;
        CancellationTokenSource m_tokenSource;
        IModel m_mqModel;
        IConnection m_mqClientConn;

        public event MessageHandler MessageReceived;

        private string m_exchange;

        public MessageEndpoint(string exchange)
        {
            m_exchange = exchange;
            m_subsDict = new Dictionary<string, Subscription>();
            m_typeQueueDict = new Dictionary<Type, string>();
            m_tokenSource = new CancellationTokenSource();

            var cf = new ConnectionFactory();

            m_mqClientConn = cf.CreateConnection();
            m_mqModel = m_mqClientConn.CreateModel();
        }

        public void Subscribe(string queueName, string key)
        {
            m_mqModel.QueueDeclare(queueName, false, false, false, null);

            m_mqModel.QueueBind(queueName, m_exchange, key);

            var subs = new RabbitMQ.Client.MessagePatterns.Subscription(m_mqModel, queueName);

            m_subsDict.Add(queueName, subs);

            Task.Factory.StartNew((o) =>
                {
                    Console.WriteLine("Waiting for msgs...");
                    BasicDeliverEventArgs args;
                    do
                    {
                        if (subs.Next(10000, out args))
                        {
                            QuickFix.Message msg = new QuickFix.Message(Encoding.Default.GetString(args.Body));

                            if (MessageReceived != null)
                                MessageReceived(msg);

                            Console.WriteLine(msg);
                        }
                    }
                    while (!m_tokenSource.Token.IsCancellationRequested);

                }, m_tokenSource.Token, TaskCreationOptions.LongRunning);
        }

        public void RegisterPublishType(Type type, string routingKey)
        {
            m_typeQueueDict.Add(type, routingKey);
        }

        public void Publish(Message msg)
        {
            IBasicProperties props = m_mqModel.CreateBasicProperties();

            var routingKey = m_typeQueueDict[msg.GetType()];

            m_mqModel.BasicPublish(m_exchange, routingKey, props, Encoding.Default.GetBytes(msg.ToString()));
        }

        #region Disposal
        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                m_mqModel.Dispose();
                GC.SuppressFinalize(this);
            }

        }

        protected void Finalize()
        {
            Dispose(false);
        } 
        #endregion
    }
}

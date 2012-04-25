using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NewOrderMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var cf = new ConnectionFactory();

            var m_mqClientConn = cf.CreateConnection();

            var m_mqModel = m_mqClientConn.CreateModel();

            var procId = System.Diagnostics.Process.GetCurrentProcess().Id;

            m_mqModel.QueueDeclare(procId.ToString(), true, false, true, null);

            m_mqModel.QueueBind(procId.ToString(), "Hitchin", string.Format("{0}.{1}.{2}", "HITCHIN", "NewOrderMessage", "#"));

            var subs = new RabbitMQ.Client.MessagePatterns.Subscription(m_mqModel, procId.ToString());

            foreach (BasicDeliverEventArgs ev in subs)
            {
                Console.WriteLine(ev.ToString());
            }
        }
    }
}

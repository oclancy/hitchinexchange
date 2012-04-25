using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using QuickFix;
using System.Reflection;

namespace HitchinExchange
{
    public class Exchange : Application, IDisposable
    {
        private bool m_disposed = false;
        private string m_appName;

        RabbitMQ.Client.IConnection m_mqClientConn;
        RabbitMQ.Client.IModel m_mqModel;

        public Exchange() 
        {
            m_appName = Assembly.GetEntryAssembly().GetName().Name;
            var cf = new ConnectionFactory();

            m_mqClientConn = cf.CreateConnection();

            m_mqModel = m_mqClientConn.CreateModel();

            m_mqModel.ExchangeDeclare("Hitchin", ExchangeType.Topic);

            m_mqModel.QueueDeclare("Exchange", true, false, false, null);

            m_mqModel.QueueBind("Exchange", "Hitchin", string.Empty);
        }

        public void fromAdmin(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromAdmin Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));
        }

        public void fromApp(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromApp Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));

            m_mqModel.BasicPublish(new PublicationAddress(ExchangeType.Topic, "Hitchin", string.Format("{0}.{1}.{2}", "HITCHIN", value.GetType().Name, sessionId)),
                                    m_mqModel.CreateBasicProperties(),
                                    Encoding.Default.GetBytes(value.ToString()));

        }

        public void onCreate(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Creating session: {0}", m_appName, value));
        }

        public void onLogon(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Session logged in: {0}", m_appName, value));
        }

        public void onLogout(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Session logged out: {0}", m_appName, value));
        }

        public void toAdmin(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:toAdmin Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));
        }

        public void toApp(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:toApp Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));

            
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
                }

                m_mqModel.Close();
                m_mqClientConn.Close();
                // Set large fields to null.
                m_disposed = true;
            }
        }

        ~Exchange()
        {
            Dispose(false);
        }
    }
}

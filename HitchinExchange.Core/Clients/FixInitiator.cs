using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;

namespace HitchinExchange.Core
{
    public class MessageAndSession : EventArgs
    {
        public Message Message{get;set;}
        public SessionID SessionId { get; set; }
    }

    public class FixInitiator : Application
    {
        SocketInitiator m_initiator;

        string m_appName;

        public event EventHandler<MessageAndSession> MessageReceived;

        public FixInitiator(string configFile, 
                            string appName)
        {
            m_appName = appName;

            SessionSettings settings = new SessionSettings(configFile);

            FileStoreFactory storeFactory = new FileStoreFactory(settings);

            ScreenLogFactory logFactory = new ScreenLogFactory(settings);

            MessageFactory messageFactory = new DefaultMessageFactory();

            m_initiator = new SocketInitiator(this, storeFactory, settings, logFactory, messageFactory);
        }

        public void fromAdmin(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromAdmin Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));
        }

        public void fromApp(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromApp Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));

            if(MessageReceived != null)
                MessageReceived(this, new MessageAndSession() { Message = value, SessionId = sessionId });
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

        public void Start()
        {
            m_initiator.start();
        }

        public void Stop()
        {
            m_initiator.stop();
        }
    }
}


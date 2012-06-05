using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;

namespace HitchinExchange.Core
{
    public class FixAcceptor : Application
    {
        SocketAcceptor m_acceptor;
        Action<Message, SessionID> m_onMessageCallBack;

        List<SessionID> m_loggedOnSessions = new List<SessionID>();

        string m_appName;

        public FixAcceptor(string configFile, 
                               string appName,
                               Action<Message, SessionID> onMessageCallBack)
        {
            m_appName = appName;

            m_onMessageCallBack = onMessageCallBack;

            SessionSettings settings = new SessionSettings(configFile);

            FileStoreFactory storeFactory = new FileStoreFactory(settings);

            ScreenLogFactory logFactory = new ScreenLogFactory(settings);

            MessageFactory messageFactory = new DefaultMessageFactory();

            m_acceptor = new SocketAcceptor(this, storeFactory, settings, logFactory, messageFactory);
        }

        public void fromAdmin(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromAdmin Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));
        }

        public void fromApp(QuickFix.Message value, QuickFix.SessionID sessionId)
        {
            Console.WriteLine(string.Format("{0}:fromApp Called with: {1}, from {2}", m_appName, value.GetType().Name, sessionId));

            m_onMessageCallBack(value, sessionId);
        }

        public void onCreate(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Creating session: {0}", m_appName, value));
        }

        public void onLogon(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Session logged in: {0}", m_appName, value));
            m_loggedOnSessions.Add(value);
        }

        public void onLogout(QuickFix.SessionID value)
        {
            Console.WriteLine(string.Format("{0}:Session logged out: {0}", m_appName, value));
            m_loggedOnSessions.Remove(value);
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
            m_acceptor.start();
        }

        public void Stop()
        {
            m_acceptor.stop();
        }

        public bool HasSessions
        {
            get
            {
                return  m_loggedOnSessions.Count > 0;
            }
        }

        public void SendMessage(Message e)
        {
            m_loggedOnSessions.ForEach( sess => Session.sendToTarget(e,sess));
        }
    }
}


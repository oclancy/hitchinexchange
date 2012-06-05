using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickFix;
using System.Threading.Tasks;

namespace HitchinExchange.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow //: Window, QuickFix.Application 
    {
        //SocketInitiator m_initiator;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //StartFixClient();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //_initiator.stop();
        }

        private void StartFixClient()
        {
            try
            {
                SessionSettings settings = new SessionSettings("Client.cfg");
                FileStoreFactory storeFactory = new FileStoreFactory(settings);
                FileLogFactory logFactory = new FileLogFactory(settings);
                MessageFactory messageFactory = new DefaultMessageFactory();
                //m_initiator = new SocketInitiator(this, storeFactory, settings, logFactory /*optional*/, messageFactory);
              //  m_initiator.start();
            }
            catch (ConfigError e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            //m_initiator.stop();
        }

        #region Fix application impl
        public void fromAdmin(Message value, SessionID sessionId)
        {
            Console.WriteLine(string.Format("fromAdmin Called with: {0}, from {1}", value.GetType().Name, sessionId));
        }

        public void fromApp(Message value, SessionID sessionId)
        {
            Console.WriteLine(string.Format("onCreate called with {0}", value));
        }

        public void onCreate(SessionID value)
        {
            Console.WriteLine(string.Format("onCreate called with {0}", value));
        }

        public void onLogon(SessionID value)
        {
            Console.WriteLine(string.Format("onLogon called with {0}", value));
        }

        public void onLogout(SessionID value)
        {
            Console.WriteLine(string.Format("onLogout called with {0}", value));
        }

        public void toAdmin(Message value, SessionID sessionId)
        {
            Console.WriteLine(string.Format("toAdmin called with {0}, with Session {1}", value.GetType().Name, sessionId));
        }

        public void toApp(Message value, SessionID sessionId)
        {
            Console.WriteLine(string.Format("toApp called with {0}, with Session {1}", value.GetType().Name, sessionId));
        } 
        #endregion

        //private void SendMessage(object sender, RoutedEventArgs e)
        //{
        //    foreach( var session in m_initiator.getSessions())
        //    {
        //        QuickFix42.NewOrderSingle order = new QuickFix42.NewOrderSingle(
        //            new ClOrdID("DLF"),
        //            new HandlInst(HandlInst.MANUAL), 
        //            new Symbol("DLF"), 
        //            new Side(Side.BUY),
        //            new TransactTime(DateTime.Now), 
        //            new OrdType(OrdType.LIMIT));

        //            order.set(new OrderQty(45));
        //            order.set(new Price(25.4d));

        //        Session.sendToTarget(order, (SessionID)session); 
        //    }
        //}
    }
}

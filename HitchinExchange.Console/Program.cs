using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;

namespace HitchinExchange.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            SessionSettings settings = new SessionSettings(@"Exchange.cfg");

            Exchange application = new Exchange();

            FileStoreFactory storeFactory = new FileStoreFactory(settings);

            ScreenLogFactory logFactory = new ScreenLogFactory(settings);

            MessageFactory messageFactory = new DefaultMessageFactory();

            SocketAcceptor acceptor = new SocketAcceptor(application, storeFactory, settings, logFactory, messageFactory);

            acceptor.start();

            System.Console.WriteLine("press <enter> to quit");

            System.Console.Read();

            acceptor.stop();
        }
    }
}

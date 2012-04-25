using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using HitchinExchange.Core;

namespace HitchinExchange.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var dirCat = new DirectoryCatalog(@".\");
            var isolatingCatalog = new IsolatingCatalog(dirCat);
            var container = new CompositionContainer(isolatingCatalog);
            var messageHandlerMediator = new MessageHandlerMediator();

            container.ComposeParts(messageHandlerMediator);

            messageHandlerMediator.StartHandlers();

            #region QuickFix
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
            #endregion
        }
    }
}

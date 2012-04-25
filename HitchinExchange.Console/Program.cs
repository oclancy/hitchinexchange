using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using HitchinExchange.Core;
using QuickFix;

namespace HitchinExchange.Console
{
    class Program
    {
        static Exchange m_exchange;
        static void Main(string[] args)
        {

            var dirCat = new DirectoryCatalog(@".\");
            var isolatingCatalog = new IsolatingCatalog(dirCat);
            var container = new CompositionContainer(isolatingCatalog);
            var messageHandlerMediator = new MessageHandlerMediator();

            container.ComposeParts(messageHandlerMediator);

            messageHandlerMediator.StartHandlers();

            m_exchange = new Exchange();

            m_exchange.Endpoint.Start();

            System.Console.WriteLine("press <enter> to quit");

            System.Console.Read();

            m_exchange.Endpoint.Stop();
        }
    }
}

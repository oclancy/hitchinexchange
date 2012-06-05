using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using HitchinExchange.Core;
using QuickFix;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace HitchinExchange.Console
{
    class Program
    {
        static IUnityContainer m_container;
        static IExchange m_exchange;

        static void Main(string[] args)
        {
            InitialiseDiContainer();

            m_exchange = m_container.Resolve<IExchange>();

            var dirCat = new DirectoryCatalog(@".\");
            //var isolatingCatalog = new IsolatingCatalog(dirCat);
            //var container = new CompositionContainer(isolatingCatalog);
            var container = new CompositionContainer(dirCat);
            var messageHandlerMediator = new MessageHandlerMediator();

            container.ComposeParts(messageHandlerMediator);

            messageHandlerMediator.StartHandlers();

            System.Console.WriteLine("press <enter> to quit");

            System.Console.Read();

            //m_exchange.Endpoint.Stop();
        }

        private static void InitialiseDiContainer()
        {
            m_container = new UnityContainer();

            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.config");
            Array.ForEach(files, (file) =>
            {
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = file };
                System.Configuration.Configuration configuration =
                        ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

                if (unitySection != null)
                    m_container.LoadConfiguration(unitySection);

            });

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(m_container));
        }
    }
}

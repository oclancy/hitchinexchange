using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;
using HitchinExchange.Core.Clients;
using System.Threading;
using QuickFix;

namespace HitchinExchange.UI
{
    public class MessageModel : INotifyPropertyChanged
    {
        private static string m_dataDictDir = @"d:\quickfix\spec";
        private XDocument m_currentFixDoc;
        private IDictionary<string, FieldType> m_fieldTypes;
        private IEnumerable<MessagePropertyDescription> m_currentMessageProperties;

        public static IEnumerable<string> KnownMessages
        {
            get
            {
                return new List<string>()
                {
                    "QuoteRequest",
                    "Quote",
                    "MassQuote",
                    "QuoteAcknowledgement",
                    "BusinessRejectMessage",
                    "NewOrderSingle",
                    "ExecutionReport",
                    "DontKnowTrade",
                    "OrderStatusRequest",
                    "OrderStatusReponse",
                    "SecurityStatusRequest",
                    "SecurityStatus"
                };
            }
        }
        public IEnumerable<string> FixVersions
        {
            get
            {
                Regex fileFilter = new Regex(@"FIX(\d)+.xml");

                return Directory.GetFiles(m_dataDictDir)
                                .Where(file => fileFilter.IsMatch(file));
            }
        }

        private string m_selectedVersion;
        public string SelectedVersion
        {
            get
            {
                return m_selectedVersion;
            }
            set
            {
                m_selectedVersion = value;
                m_currentFixDoc = XDocument.Load( Path.Combine(m_dataDictDir, m_selectedVersion));

                // load field types
                if(m_fieldTypes!=null)m_fieldTypes.Clear();
                m_fieldTypes = m_currentFixDoc.Element("fix")
                                              .Element("fields")
                                              .Elements()
                                              .ToDictionary(ele => ele.Attribute("name").Value,
                                                                   ele => new FieldType()
                                                                   {
                                                                       Name = ele.Attribute("name").Value,
                                                                       FixType = ele.Attribute("type").Value,
                                                                       Number = int.Parse(ele.Attribute("number").Value),
                                                                       Values = ele.Elements()
                                                                                   .Select(val => new FieldValue()
                                                                                   {
                                                                                       DisplayValue = val.Attribute("description").Value,
                                                                                       ActualValue = val.Attribute("enum").Value
                                                                                   })
                                                                                   .ToList()
                                                                   });

                PropertyChanged.NotifyPropertyChanged(()=>SelectedVersion);
                PropertyChanged.NotifyPropertyChanged(()=>MessagesForFixVersion);
            }
        }

        public IEnumerable<string> MessagesForFixVersion
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedVersion)) return null;

                var list =  m_currentFixDoc.Element("fix")
                                      .Element(@"messages")
                                      .Elements(@"message")
                                      .Select( ele => ele.Attribute("name").Value )
                                      .Where( msgName => KnownMessages.Contains(msgName) )
                                      .ToList();

                return list;
            }
        }

        private string m_selectedMessage;
        public string SelectedMessage
        {
            get
            {
                return m_selectedMessage;
            }
            set
            {
                m_selectedMessage = value;

                PropertyChanged.NotifyPropertyChanged(() => SelectedMessage);
                PropertyChanged.NotifyPropertyChanged(() => Properties);
            }
        }

        public IEnumerable<MessagePropertyDescription> Properties
        {
            get
            {
                m_currentMessageProperties = m_currentFixDoc.Element("fix")
                                        .Element("messages")
                                        .Elements()
                                        .First(ele => ele.FirstAttribute.Value == m_selectedMessage)
                                        .Elements()
                                        .Select(ele => new MessagePropertyDescription()
                                                            {
                                                                Name = ele.FirstAttribute.Value,
                                                                Value = string.Empty,
                                                                FieldType = m_fieldTypes[ele.Attribute("name").Value],
                                                                IsRequired = ele.Attribute("required").Value == "Y" ? true :false,
                                                            });

                return m_currentMessageProperties;
            }
        }

        public bool IsValidMessage
        {
            get
            {
                return m_currentMessageProperties.All(mesgProp => mesgProp.IsRequired == false || mesgProp.Value != null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        MessageEndpoint m_messageEndpoint;
        CancellationToken m_cancelToken;

        public MessageModel()
        {
            var css = new CancellationTokenSource();
            m_cancelToken = css.Token;

            m_messageEndpoint = new MessageEndpoint("Hitchin");

            m_messageEndpoint.RegisterPublishType(typeof(QuickFix42.NewOrderSingle),
                                                   "Messages.NewOrderSingle.42");


            m_messageEndpoint.Subscribe("GUI", "Messages.ExecutionReport.*");
            m_messageEndpoint.Subscribe("GUI", "Messages.Quote.*");

            m_messageEndpoint.MessageReceived += m_messageEndpoint_MessageReceived;

            Task.Factory.StartNew( () =>
                {
                    while (!m_cancelToken.IsCancellationRequested)
                    {
                        var msg = new QuickFix42.NewOrderSingle( new ClOrdID("clientOrderId"),
                                                                 new HandlInst(HandlInst.AUTOEXECPUB),
                                                                 new Symbol("VOD"),
                                                                 new Side(Side.BUY),
                                                                 new TransactTime(DateTime.Now),
                                                                 new OrdType(OrdType.MARKET));


                        m_messageEndpoint.Publish(msg);
                        Thread.Sleep(10000);
                    }
                }, m_cancelToken);
        }

        void m_messageEndpoint_MessageReceived(QuickFix.Message msg)
        {
            Console.WriteLine("Message received {0}",msg);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using HitchinExchange;
using HitchinExchange.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Reactive.Linq;
using QuickFix;
using QuickFix42;
using HitchinExchange.Core.Clients;

namespace NewOrderMessageHandler
{
    [Isolation(typeof(IProcessFixMessages), Isolate = true)]
    public class NewOrderHandler : MarshalByRefObject, IProcessFixMessages
    {
        private FixInitiator m_fixEndPoint;
        private MessageEndpoint m_messageEndpoint;
        
        private void OnMessage(QuickFix.Message message, SessionID sessionId)
        {
            // message received from RSP
            Console.WriteLine("Received order");

            Session.sendToTarget(new QuickFix42.ExecutionReport());

            Console.WriteLine("Execution Report published");

            //Session.sendToTarget(new QuickFix42.Quote());
            
        }

        public void Start()
        {
             m_fixEndPoint = new FixInitiator("NewOrderMessageHandler.cfg",
                                              GetType().Name,
                                              OnMessage);

             //m_messageEndpoint.MessageReceived += new MessageEndpoint.MessageHandler(m_messageEndpoint_MessageReceived);
             //m_messageEndpoint.Subscribe("NewOrderHandler", "Message.NewOrderSingle.*");

             //m_messageEndpoint.RegisterPublishType(typeof(QuickFix42.ExecutionReport), "Message.ExecutionReport.42");
        }

        void m_messageEndpoint_MessageReceived(QuickFix.Message msg)
        {
  
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

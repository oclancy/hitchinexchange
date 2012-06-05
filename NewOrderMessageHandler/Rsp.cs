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

namespace Rsp
{
    [Isolation(typeof(IProcessFixMessages), Isolate = true)]
    public class Rsp : MarshalByRefObject, IProcessFixMessages
    {
        private FixInitiator m_fixEndPoint;
        private QuickFix.MessageCracker m_cracker;

        private void OnMessage(QuickFix.Message message, SessionID sessionId)
        {
            HitchinExchange.Core.Cracker.MessageCracker.Crack(message);
            // message received from Exchange
            Console.WriteLine("Received message");

            //Session.sendToTarget(new QuickFix42.ExecutionReport(), sessionId);

            Console.WriteLine("Execution Report published");

            var quote = new QuickFix42.Quote( new QuoteID( (message as NewOrderSingle).getClOrdID().getValue() ),
                                              new Symbol("VOD"));


            Session.sendToTarget( quote, sessionId);
            
        }

        public void Start()
        {
            m_fixEndPoint = new FixInitiator("NewOrderMessageHandler.cfg",
                                              GetType().Name);

            var observable = Observable.FromEvent<EventHandler<MessageAndSession>, MessageAndSession>( ev => m_fixEndPoint.MessageReceived += ev,
                                                                                                       ev => m_fixEndPoint.MessageReceived -= ev );

            observable.Subscribe(mands =>
                {
                    if (mands.Message.getField(QuickFix.MsgType.FIELD) != "D")
                        throw new ArgumentException();

                    Console.WriteLine(mands.Message);
                }, 
                (Exception e) => { Console.WriteLine(e.Message,e.Source); });

             m_fixEndPoint.Start();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

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

namespace NewOrderMessageHandler
{
    [Isolation(typeof(IProcessFixMessages), Isolate = true)]
    public class NewOrderHandler : MarshalByRefObject, IProcessFixMessages
    {
        public void Start()
        {
             var t1 = Task.Factory.StartNew(() =>
                {
                    var cf = new ConnectionFactory();

                    using (var m_mqClientConn = cf.CreateConnection())
                    {
                        using (var m_mqModel = m_mqClientConn.CreateModel())
                        {

                            m_mqModel.QueueDeclare("NewOrderHandler", true, false, false, null);

                            m_mqModel.QueueBind("NewOrderHandler", "Hitchin", string.Format("{0}.{1}.{2}", "HITCHIN", "NewOrderSingle", "#"));

                            var subs = new RabbitMQ.Client.MessagePatterns.Subscription(m_mqModel, "NewOrderHandler");

                            Console.WriteLine("Waiting for msgs...");

                            BasicDeliverEventArgs args;
                            while (subs.Next(15000, out args) == true)
                            {
                                QuickFix.Message msg = new QuickFix.Message( Encoding.Default.GetString(args.Body) );

                                NewOrderSingleMessageCracker cracker = new NewOrderSingleMessageCracker();
                                var sessionId = new SessionID();
                                sessionId.fromString(Encoding.Default.GetString(args.BasicProperties.Headers["QFSessionId"] as byte[]));
                                cracker.crack(msg, sessionId);
                                
                                Console.WriteLine(msg);

                            }
                        }
                    }
                });

            t1.ContinueWith(t=> Console.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            t1.ContinueWith(t => Console.WriteLine("No Problems"), TaskContinuationOptions.NotOnFaulted);
            
        }

        public void Dispose()
        {
            
        }
    }
}

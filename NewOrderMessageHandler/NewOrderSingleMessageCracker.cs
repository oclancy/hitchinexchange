using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;

namespace NewOrderMessageHandler
{
    class NewOrderSingleMessageCracker : QuickFix.MessageCracker
    {
        public new void onMessage(QuickFix.Message message, QuickFix.SessionID session)
        {
            string msgType = message.getHeader().getString(35);

            if (msgType.Equals(MsgType.NewOrderSingle))
            {
            }
        }

        public override void onMessage(QuickFix42.NewOrderSingle message, QuickFix.SessionID session)
        {

        }

        public override void onMessage(QuickFix41.NewOrderSingle message, QuickFix.SessionID session)
        {
        }

        public override void onMessage(QuickFix40.NewOrderSingle message, QuickFix.SessionID session)
        {
        }

        public override void onMessage(QuickFix43.NewOrderSingle message, QuickFix.SessionID session)
        {
        }


        public override void onMessage(QuickFix44.NewOrderSingle message, QuickFix.SessionID session)
        {
        }


        public override void onMessage(QuickFix50.NewOrderSingle message, QuickFix.SessionID session)
        {
        }

    }
}

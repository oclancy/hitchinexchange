using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;

namespace HitchinExchange.Core.Cracker
{
    public class MessageCracker
    {
        static dynamic Crack(QuickFix.Message msg, string typename)
        {
            dynamic obj = Activator.CreateInstance("quick_fix_net_messages", typename);

            obj.setString(msg.ToString());

            return obj;
        }

        public static void Crack(QuickFix.Message message)
        {
           message.getString(
        }
    }
}

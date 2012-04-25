using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using QuickFix;

namespace HitchinExchange.Core
{
    public abstract class MessageCrackerEx<>
    {
        private Dictionary<Type, MethodInfo> _handlerMethods = new Dictionary<Type, MethodInfo>();

        public MessageCrackerEx()
        {
            initialize(this);
        }

        private void initialize(Object messageHandler)
        {
            Type handlerType = messageHandler.GetType();

            MethodInfo[] methods = handlerType.GetMethods();
            foreach (MethodInfo m in methods)
            {
                if (IsHandlerMethod(m))
                {
                    _handlerMethods[m.GetParameters()[0].ParameterType] = m;
                }
            }
        }


        static public bool IsHandlerMethod(MethodInfo m)
        {
            return (m.IsPublic == true
                && m.Name.Equals("OnMessage")
                && m.GetParameters().Length == 2
                && m.GetParameters()[0].ParameterType.IsSubclassOf(typeof(QuickFix.Message))
                && typeof(QuickFix.SessionID).IsAssignableFrom(m.GetParameters()[1].ParameterType)
                && m.ReturnType == typeof(void));
        }


        /// <summary>
        /// Process ("crack") a FIX message and call the registered handlers for that type, if any
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void Crack(Message message, SessionID sessionID)
        {
            Type messageType = message.GetType();

            MethodInfo handler = null;

            if (_handlerMethods.TryGetValue(messageType, out handler))
                handler.Invoke(this, new object[] { message, sessionID });
            else if( messageType == typeof(QuickFix.Message) )
            {
                var fixVersion = message.getHeader().getString(BeginString.FIELD);
                var qualifiedMsg = "Quick" + fixVersion.Replace('.','') + "." + ;

                _handlerMethods.First( kvp => string.Compare(kvp.Key.FullName, qualifiedMsg, true) );
            }
            else
                throw new UnsupportedMessageType();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using HitchinExchange.Core;

namespace HitchinExchange
{
    public class MessageHandlerMediator
    {
        [ImportMany(typeof(IProcessFixMessages), AllowRecomposition = true)]
        public IEnumerable<IProcessFixMessages> FixMessageProcessors
        {
            get;
            set;
        }

        public void StartHandlers()
        {
            FixMessageProcessors.ForEach(i => i.Start());
        }
    }
}

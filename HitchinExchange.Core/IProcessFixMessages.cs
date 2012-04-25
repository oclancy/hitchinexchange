using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HitchinExchange
{
    public interface IProcessFixMessages : IDisposable
    {
        void Start();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace HitchinExchange.Core
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IsolationAttribute : ExportAttribute
    {
        public IsolationAttribute(Type contractType) : base(contractType) 
        {
            Isolate = true;
        }

        public bool Isolate { get; set; }
    }
}

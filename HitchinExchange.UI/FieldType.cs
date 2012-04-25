using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HitchinExchange.UI
{
    public class FieldValue
    {
        public string DisplayValue { get; set; }
        public string ActualValue { get; set; }
    }

    public class FieldType
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public IEnumerable<FieldValue> Values { get; set; }

        public string FixType { get; set; }
    }
}

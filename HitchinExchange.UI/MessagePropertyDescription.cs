using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HitchinExchange.UI
{
    public class MessagePropertyDescription : IDataErrorInfo
    {
        public string Name { get; set; }

        public FieldType FieldType { get; set; }
        
        public object Value { get; set; }
        
        public bool IsRequired { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get 
            {
                if (Value == string.Empty && IsRequired == true)
                    return "This field is required";

                return null;
            }
        }
    }
}

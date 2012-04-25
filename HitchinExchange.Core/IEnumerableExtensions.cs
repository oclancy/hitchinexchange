using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HitchinExchange.Core
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>( this IEnumerable<T> enumearableOfT, Action<T> action )
        {
            foreach (var item in enumearableOfT)
            {
                action(item);
            }
        }
    }
}

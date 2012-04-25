using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;

namespace HitchinExchange.UI
{
    static class ViewModelExension
    {
        public static void NotifyPropertyChanged(this PropertyChangedEventHandler handler, Expression<Func<object>> expr)
        {
            var body = expr.Body as MemberExpression;
            var expression = body.Expression as ConstantExpression;
            handler(expression.Value, new PropertyChangedEventArgs(body.Member.Name));
        }
    }
}

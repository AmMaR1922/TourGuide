using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Models
{
    public class OrderExpression<T> where T : class
    {
        public Expression<Func<T, object>> KeySelector { get; set; } = null!;
        public bool IsDescending { get; set; } 

        public OrderExpression(Expression<Func<T, object>> KeySelector, bool IsDescending)
        {
            this.KeySelector = KeySelector;
            this.IsDescending = IsDescending;
        }
    }
}

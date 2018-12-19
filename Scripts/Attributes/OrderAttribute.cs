
using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class OrderAttribute : Attribute
    {
        public int Order;
        
        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}

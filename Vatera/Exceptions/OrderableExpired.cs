using System;
using Vatera.Interfaces;

namespace Vatera.Exceptions
{
    class OrderableExpired: Exception
    {
        public IExpiringOrderable Orderable;

        public OrderableExpired(IExpiringOrderable orderable)
        {
            Orderable = orderable;
        }
    }
}

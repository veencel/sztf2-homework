using System;
using Vatera.Interfaces;

namespace Vatera.Models
{
    class Order
    {
        public Customer Customer { get; }
        public WeakReference<IOrderable> Orderable { get; }
        public int Count { get; }

        public Order(Customer customer, IOrderable orderable, int count = 1)
        {
            Customer = customer;
            Orderable = new WeakReference<IOrderable>(orderable);
            Count = count;
        }
    }
}

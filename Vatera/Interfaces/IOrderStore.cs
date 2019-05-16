using System;
using System.Collections.Generic;
using Vatera.Models;

namespace Vatera.Interfaces
{
    public interface IOrderStore: IEnumerable<Order>
    {
        int Count { get; }

        void Insert(Order order);

        Order Get(int index);

        void Remove(Order order);

        int Sum(Func<Order, int> valueRetriever);

        bool Contains(Order searchedOrder);

        IOrderStore Sorted(Func<Order, Order, bool> sorter);
    }
}

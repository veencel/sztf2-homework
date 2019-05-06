using System;
using System.Collections;
using System.Collections.Generic;
using Vatera.Exceptions;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera
{
    delegate void ShopEventHandler(Order order);

    class Shop
    {
        private event ShopEventHandler OrderCompleted;
        private event ShopEventHandler OrderFailed;

        public int Day = 1;

        public IOrderableStore Orderables { get; }
        public Customer[] Customers { get; }

        public Shop(IOrderableStore orderables, Customer[] customers)
        {
            Orderables = orderables;
            Customers = customers;
        }

        public void Order(Customer customer, IOrderable orderable)
        {
            GuardExpiring(orderable);

            var order = orderable.Order(customer);

            OrderCompleted(order);
        }

        public void Order(Customer customer, IOrderableWithCount orderable, int count)
        {
            GuardExpiring(orderable);

            orderable.Order(customer, count);
        }

        public Customer FindCustomer(string identifier)
        {
            foreach (var customer in Customers)
            {
                if (customer.Identifier == identifier)
                {
                    return customer;
                }
            }

            return null;
        }

        public void OnOrderCompleted(ShopEventHandler handler)
        {
            OrderCompleted += handler;
        }

        public void OnOrderFailed(ShopEventHandler handler)
        {
            OrderFailed += handler;
        }

        public AuctionResult[] GoToNextDay()
        {
            Day += 1;
            List<AuctionResult> results = new List<AuctionResult>();

            foreach (var orderable in Orderables.ToArray())
            {
                if (orderable is Product product && product.DaysToExpire == Day)
                {
                    try
                    {
                        var result = FinishAuction(product);
                        results.Add(result);
                    }
                    catch (CouldNotSellAll exception)
                    {
                        results.Add(exception.Result);
                    }
                }
            }

            return results.ToArray();
        }

        private void GuardExpiring(IOrderable orderable)
        {
            if (orderable is IExpiringOrderable expiringOrderable && expiringOrderable.DaysToExpire < Day)
            {
                throw new OrderableExpired(expiringOrderable);
            }
        }

        private AuctionResult FinishAuction(Product product)
        {
            Order[][] orders = product.Orders.GroupByRating();

            List<Order> successfulOrders = new List<Order>();
            List<Order> failedOrders = new List<Order>();

            int count = product.Count;

            for (int i = 5; i >= 1; i--)
            {
                Array.Sort(orders[i], (Order order, Order otherOrder) => otherOrder.Count - order.Count);

                foreach (var order in orders[i])
                {
                    if (count == 0)
                    {
                        failedOrders.Add(order);
                        OrderFailed(order);
                    }
                    else if (count - order.Count >= 0)
                    {
                        count -= order.Count;

                        successfulOrders.Add(order);
                        OrderCompleted(order);
                    }
                }
            }

            var result = new AuctionResult(product, successfulOrders.ToArray(), failedOrders.ToArray());

            if (count > 0)
            {
                throw new CouldNotSellAll(result);
            }

            return result;
        }
    }
}

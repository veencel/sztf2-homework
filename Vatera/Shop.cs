using System.Collections.Generic;
using Vatera.Exceptions;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera
{
    delegate void ShopEventHandler(Order order);

    class Shop
    {
        private readonly IAuctionManager _auctionManager;
        private event ShopEventHandler OrderCompleted;
        private event ShopEventHandler OrderFailed;

        public int Day = 1;

        public IOrderableStore Orderables { get; }
        public Customer[] Customers { get; }

        public Shop(IOrderableStore orderables, Customer[] customers, IAuctionManager auctionManager)
        {
            Orderables = orderables;
            Customers = customers;
            _auctionManager = auctionManager;
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
                        var result = _finishAuction(product);
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

        private AuctionResult _finishAuction(Product product)
        {
            var result = _auctionManager.FinishAuction(product);

            foreach (var order in result.SuccessfulOrders)
            {
                OrderCompleted(order);
            }

            foreach (var order in result.FailedOrders)
            {
                OrderFailed(order);
            }

            if (result.CouldNotSellAll)
            {
                throw new CouldNotSellAll(result);
            }

            return result;
        }

        private void GuardExpiring(IOrderable orderable)
        {
            if (orderable is IExpiringOrderable expiringOrderable && expiringOrderable.DaysToExpire < Day)
            {
                throw new OrderableExpired(expiringOrderable);
            }
        }
    }
}

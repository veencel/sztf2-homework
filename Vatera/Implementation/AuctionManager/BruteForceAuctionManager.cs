using System;
using Vatera.Filters;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation.AuctionManager
{
    public class BruteForceAuctionManager: IAuctionManager
    {
        public AuctionResult FinishAuction(Product product)
        {
            LinkedListOrderStore successfulOrders = null;

            for (int i = 1; i < Math.Pow(2, product.Orders.Count); ++i)
            {
                LinkedListOrderStore betterCombination = new LinkedListOrderStore();

                int j = 0;
                foreach (var order in product.Orders)
                {
                    if ((int)(i / Math.Pow(2, j++)) % 2 == 1)
                    {
                        betterCombination.Insert(order);
                    }
                }

                if (_sumOrderCount(betterCombination) <= product.Count && _sumRatings(betterCombination) > _sumRatings(successfulOrders))
                {
                    successfulOrders = betterCombination;
                }
            }

            return new AuctionResult(product, successfulOrders, FailedOrderFilter.Filter(product.Orders, successfulOrders));
        }

        private int _sumRatings(LinkedListOrderStore orders)
        {
            if (orders == null)
            {
                return 0;
            }

            return orders.Sum(order => order.Customer.Rating);
        }

        private int _sumOrderCount(LinkedListOrderStore orders)
        {
            return orders.Sum(order => order.Count);
        }
    }
}

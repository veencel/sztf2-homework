using Vatera.Filters;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation.AuctionManager
{
    public class BacktrackAuctionManager: IAuctionManager
    {
        public AuctionResult FinishAuction(Product product)
        {
            IOrderStore temp = new LinkedListOrderStore();
            IOrderStore successfulOrders = new LinkedListOrderStore();

            _backtrack(1, ref temp, ref successfulOrders, product);

            return new AuctionResult(product, successfulOrders, FailedOrderFilter.Filter(product.Orders, successfulOrders));
        }

        private void _backtrack(int level, ref IOrderStore temp, ref IOrderStore successfulOrders, Product product)
        {
            for (int i = 0; i <= 1; ++i)
            {
                if (i == 0)
                {
                    temp.Insert(product.Orders.Get(level - 1));
                }
                else
                {
                    temp.Remove(product.Orders.Get(level - 1));
                }

                if (_isPossibleSolution(level, temp, product))
                {
                    if (level == product.Orders.Count)
                    {
                        if (temp.Sum(order => order.Customer.Rating) > successfulOrders.Sum(order => order.Customer.Rating))
                        {
                            successfulOrders = new LinkedListOrderStore(temp);
                        }
                    }
                    else
                    {
                        _backtrack(level + 1, ref temp, ref successfulOrders, product);
                    }
                }
            }
        }

        private bool _isPossibleSolution(int level, IOrderStore orders, Product product)
        {
            return orders.Sum(order => order.Count) <= product.Count;
        }
    }
}

using Vatera.Filters;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation.AuctionManager
{
    public class BranchAndBoundAuctionManager: IAuctionManager
    {
        public AuctionResult FinishAuction(Product product)
        {
            IOrderStore temp = new LinkedListOrderStore();
            IOrderStore successfulOrders = new LinkedListOrderStore();

            _branchAndBound(1, ref temp, ref successfulOrders, product);
           
            return new AuctionResult(product, successfulOrders, FailedOrderFilter.Filter(product.Orders, successfulOrders));
        }

        void _branchAndBound(int level, ref IOrderStore temp, ref IOrderStore successfulOrders, Product product)
        {
            for (int i = 0; i <= 1; ++i)
            {
                if (i == 0)
                {
                    temp.Insert(product.Orders.Get(level));
                }
                else
                {
                    temp.Remove(product.Orders.Get(level));
                }

                if (_isPossibleSolution(level, temp, product))
                {
                    if (level == product.Count)
                    {
                        if (temp.Sum(order => order.Customer.Rating) > successfulOrders.Sum(order => order.Customer.Rating))
                        {
                            successfulOrders = new LinkedListOrderStore(temp);
                        }
                    }
                    else if(_shouldGoDeeper(level, temp, successfulOrders, product))
                    {
                        _branchAndBound(level + 1, ref temp, ref successfulOrders, product);
                    }
                }
            }
        }

        private bool _isPossibleSolution(int level, IOrderStore orders, Product product)
        {
            return orders.Sum(order => order.Count) <= product.Count;
        }

        private bool _shouldGoDeeper(int level, IOrderStore temp, IOrderStore successfulOrders, Product product)
        {
            return temp.Sum(order => order.Customer.Rating) + _bound(level, temp, product)
                   > successfulOrders.Sum(order => order.Customer.Rating);
        }

        private int _bound(int level, IOrderStore temp, Product product)
        {
            int result = 0;

            for (int i = level + 1; i < product.Count; ++i)
            {
                var order = product.Orders.Get(i);

                if (temp.Sum(item => item.Count) + order.Count < product.Count)
                {
                    result += order.Count;
                }
            }

            return result;
        }
    }
}

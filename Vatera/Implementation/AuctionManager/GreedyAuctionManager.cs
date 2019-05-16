using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation.AuctionManager
{
    public class GreedyAuctionManager: IAuctionManager
    {
        public AuctionResult FinishAuction(Product product)
        {
            IOrderStore sortedOrders = product.Orders.Sorted((order, otherOrder) =>
            {
                if (order.Customer.Rating == otherOrder.Customer.Rating)
                {
                    return order.Count > otherOrder.Count;
                }

                return order.Customer.Rating > otherOrder.Customer.Rating;
            });

            IOrderStore successfulOrders = new LinkedListOrderStore();
            IOrderStore failedOrders = new LinkedListOrderStore();
            int count = 0;

            foreach (var order in product.Orders)
            {
                if (count + order.Count <= product.Count)
                {
                    count += order.Count;
                    successfulOrders.Insert(order);
                }
                else
                {
                    failedOrders.Insert(order);
                }
            }

            return new AuctionResult(product, successfulOrders, failedOrders);
        }
    }
}

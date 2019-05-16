using Vatera.Interfaces;

namespace Vatera.Models
{
    public class AuctionResult
    {
        public readonly Product Product;
        public readonly IOrderStore SuccessfulOrders;
        public readonly IOrderStore FailedOrders;
        public readonly bool CouldNotSellAll;

        public AuctionResult(Product product, IOrderStore successfulOrders, IOrderStore failedOrders)
        {
            Product = product;

            SuccessfulOrders = successfulOrders;
            FailedOrders = failedOrders;

            CouldNotSellAll = Product.Count > SumSuccessfulOrders();
        }

        private int SumSuccessfulOrders()
        {
            int sum = 0;

            foreach (var order in SuccessfulOrders)
            {
                sum += order.Count;
            }

            return sum;
        }
    }
}

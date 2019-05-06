namespace Vatera.Models
{
    class AuctionResult
    {
        public Product Product;
        public Order[] SuccessfulOrders;
        public Order[] FailedOrders;
        public bool CouldNotSellAll = false;

        public AuctionResult(Product product, Order[] successfulOrders, Order[] failedOrders)
        {
            Product = product;
            SuccessfulOrders = successfulOrders;
            FailedOrders = failedOrders;
        }
    }
}

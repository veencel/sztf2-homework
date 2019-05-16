using Vatera.Implementation;
using Vatera.Interfaces;

namespace Vatera.Filters
{
    static class FailedOrderFilter
    {
        public static IOrderStore Filter(IOrderStore orders, IOrderStore successfulOrders)
        {
            IOrderStore failedOrders = new LinkedListOrderStore();

            foreach (var order in orders)
            {
                if (!successfulOrders.Contains(order))
                {
                    failedOrders.Insert(order);
                }
            }

            return failedOrders;
        }
    }
}

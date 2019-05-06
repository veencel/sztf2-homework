using Vatera.Models;

namespace Vatera.Interfaces
{
    interface IOrderStore
    {
        void Insert(Order order);
        Order[][] GroupByRating();
    }
}

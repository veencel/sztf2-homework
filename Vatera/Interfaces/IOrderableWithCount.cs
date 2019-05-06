using Vatera.Models;

namespace Vatera.Interfaces
{
    interface IOrderableWithCount: IOrderable
    {
        int Count { get; }

        IOrderStore Orders { get; }

        Order Order(Customer customer, int count);
    }
}

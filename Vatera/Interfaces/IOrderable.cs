using Vatera.Models;

namespace Vatera.Interfaces
{
    interface IOrderable
    {
        int Price { get; }

        string Identifier { get; }

        string Name { get; }

        string Description { get; }

        Order Order(Customer customer);
    }
}

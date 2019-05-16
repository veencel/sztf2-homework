using Vatera.Models;

namespace Vatera.Interfaces
{
    public interface IOrderable
    {
        int Price { get; }

        string Identifier { get; }

        string Name { get; }

        string Description { get; }

        Order Order(Customer customer);
    }
}

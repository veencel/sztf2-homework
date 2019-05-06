using Vatera.Interfaces;

namespace Vatera.Models
{
    class Service: IOrderable
    {
        public int Price { get; }

        public string Identifier { get; }

        public string Name { get; }

        public string Description
        {
            get
            {
                return this.Name + "\n\n"
                                 + "Cikkszám: " + Identifier + "\n"
                                 + "Ár: " + Price;
            }
        }

        public Service(string name, string identifier, int price)
        {
            Name = name;
            Identifier = identifier;
            Price = price;
        }

        public Order Order(Customer customer)
        {
            return new Order(customer, this);
        }
    }
}

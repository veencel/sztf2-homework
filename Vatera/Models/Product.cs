using Vatera.Implementation;
using Vatera.Interfaces;

namespace Vatera.Models
{
    class Product: IOrderableWithCount, IExpiringOrderable
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
                                 + "Ár: " + Price + "\n"
                                 + "Elérhető darabszám: " + Count + "\n"
                                 + "Hátralevő napok száma: " + DaysToExpire;
            }
        }

        public int Count { get; }

        public int DaysToExpire { get; set; }

        public IOrderStore Orders { get; }

        public Product(string name, string identifier, int price, int count, int daysToExpire)
        {
            Name = name;
            Identifier = identifier;
            Price = price;
            Count = count;
            DaysToExpire = daysToExpire;

            Orders = new LinkedListOrderStore();
        }

        public Order Order(Customer customer)
        {
            return Order(customer, 1);
        }

        public Order Order(Customer customer, int count)
        {
            var order = new Order(customer, this, count);

            Orders.Insert(order);

            return order;
        }
    }
}

namespace Vatera.Models
{
    class Customer
    {
        public string Name { get; }
        public string Identifier { get; }
        public int Rating { get; }

        public Customer(string name, string identifier, int rating)
        {
            Name = name;
            Identifier = identifier;
            Rating = rating;
        }
    }
}

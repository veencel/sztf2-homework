using System;
using Vatera.FileReaders;
using Vatera.Implementation;
using Vatera.Implementation.AuctionManager;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera
{
    class Program
    {
        static void Main(string[] args)
        {               
            Shop shop = new Shop(LoadOrderables(), LoadCustomers(), new GreedyAuctionManager());

            ShopConsoleInterface consoleInterface = new ShopConsoleInterface(shop);

            consoleInterface.Run();
        }

        private static Customer[] LoadCustomers()
        {
            try
            {
                return CustomerReader.Read("customers.txt");
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("A customers.txt nem található! " + exception.Message);
            }

            return new Customer[0];
        }

        static IOrderableStore LoadOrderables()
        {
            IOrderableStore orderables = new BinarySearchTreeOrderableStore();

            try
            {
                Product[] products = ProductReader.Read("products.txt");

                foreach (var product in products)
                {
                    orderables.Insert(product);
                }
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("A products.txt nem található! " + exception.Message);
            }

            try
            {
                Service[] services = ServiceReader.Read("services.txt");

                foreach (var service in services)
                {
                    orderables.Insert(service);
                }
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("A services.txt nem található! " + exception.Message);
            }

            return orderables;
        }
    }
}

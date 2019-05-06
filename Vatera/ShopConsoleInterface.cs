using System;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera
{
    class ShopConsoleInterface
    {
        private readonly Shop _shop;
        private Customer _customer;

        public ShopConsoleInterface(Shop shop)
        {
            _shop = shop;

            _shop.OnOrderCompleted(HandleSuccessfulOrder);
            _shop.OnOrderFailed(HandleFailedOrder);
        }

        public void Run()
        {
            ShowWelcomeMessage();
            Authenticate();

            while (true)
            {
                Console.WriteLine();
                ShowMenu();
                Console.WriteLine();
                Console.Write("Válassz: ");
                string operation = Console.ReadLine();

                try
                {
                    switch (int.Parse(operation))
                    {
                        case 1:
                            AddProduct();
                            break;
                        case 2:
                            AddService();
                            break;
                        case 3:
                            DeleteOrderable();
                            break;
                        case 4:
                            ListOrderables();
                            break;
                        case 5:
                            SearchByIdentifier();
                            break;
                        case 6:
                            SearchByName();
                            break;
                        case 7:
                            GoToNextDay();
                            break;
                        case 8:
                            _customer = null;
                            Console.WriteLine();
                            Authenticate();
                            break;
                        case 9:
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Érvénytelen utasítás!");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (FormatException exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hibás formátum!");
                    Console.ResetColor();
                }

            }
        }
       
        private void ShowMenu()
        {
            Console.WriteLine("Válassz az alábbiak közül");
            Console.WriteLine();

            Console.WriteLine("1. Termék hozzáadása");
            Console.WriteLine("2. Szolgáltatás hozzáadása");
            Console.WriteLine("3. Termék vagy szolgáltatás törlése");
            Console.WriteLine("4. Termékek/szolgáltatások listázása");
            Console.WriteLine("5. Keresés cikkszám alapján");
            Console.WriteLine("6. Keresés név szerint");
            Console.WriteLine("7. Ugrás a következő napra");
            Console.WriteLine();
            Console.WriteLine("8. Kijelentkezés");
            Console.WriteLine("9. Kilépés");
        }

        private void ShowWelcomeMessage()
        {
            Console.WriteLine("Üdvözöllek a Vatera-szerű online vásárlási felületen!");
        }

        private void Authenticate()
        {
            Console.WriteLine("Kérlek add meg az azonosítódat!");
            Console.WriteLine();
            Console.Write("Ügyfélazonosító: ");

            string identifier = Console.ReadLine();

            _customer = _shop.FindCustomer(identifier);

            if (_customer == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A megadott ügyfélazonosító nem található az adatbázisunkban!");
                Console.ResetColor();
                Console.WriteLine();

                Authenticate();
                return;
            }

            Console.WriteLine("Sikeres azonosítás: " + _customer.Name);
            Console.WriteLine();
        }

        private void AddProduct()
        {
            Console.WriteLine();
            Console.WriteLine("Adja meg a termék adatait!");
            Console.Write("Név: ");
            string name = Console.ReadLine();

            Console.Write("Cikkszám: ");
            string identifier = Console.ReadLine();

            Console.Write("Ár: ");
            int price = int.Parse(Console.ReadLine());

            Console.Write("Darabszám: ");
            int count = int.Parse(Console.ReadLine());
;
            Console.Write("Lejárati idő (napok száma): ");
            int days = int.Parse(Console.ReadLine());

            _shop.Orderables.Insert(new Product(name, identifier, price, count, days));

            Console.WriteLine();
            Console.WriteLine("Sikeres mentés!");
        }

        private void AddService()
        {
            Console.WriteLine();
            Console.WriteLine("Adja meg a szolgáltatás adatait!");
            Console.Write("Név: ");
            string name = Console.ReadLine();

            Console.Write("Cikkszám: ");
            string identifier = Console.ReadLine();

            Console.Write("Ár: ");
            int price = int.Parse(Console.ReadLine());

            _shop.Orderables.Insert(new Service(name, identifier, price));

            Console.WriteLine();
            Console.WriteLine("Sikeres mentés!");
        }

        private void DeleteOrderable()
        {
            Console.WriteLine();
            Console.WriteLine("Adja meg a törölni kívánt termék/szolgáltatás cikkszámát: ");

            string identifier = Console.ReadLine();

            var orderable = _shop.Orderables.BinarySearch(identifier);

            if (orderable == null)
            {
                Console.WriteLine("A megadott cikkszámmal nem található termék/szolgáltatás az adatbázisunkban!");
                return;
            }

            _shop.Orderables.Remove(identifier);

            Console.WriteLine();
            Console.WriteLine("Sikeres törlés!");
        }

        private void ListOrderables()
        {
            Console.WriteLine();

            foreach (var orderable in _shop.Orderables.ToArray())
            {
                Console.WriteLine(orderable.Identifier + " : " + orderable.Name);
            }

            Console.WriteLine();
            Console.WriteLine("írja be a megrendelni kívánt termék cikkszámát vagy nyomjon entert!");

            string identifier = Console.ReadLine();

            if (identifier == "")
            {
                return;
            }

            Buy(identifier);
        }

        private void SearchByIdentifier()
        {
            Console.WriteLine();
            Console.Write("Adja meg a keresett termék/szolgáltatás cikkszámát: ");

            string identifier = Console.ReadLine();

            var orderable = _shop.Orderables.BinarySearch(identifier);

            if (orderable == null)
            {
                Console.WriteLine("A megadott cikkszámmal nem található termék/szolgáltatás az adatbázisunkban!");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("A termék/szolgáltatás adatai:");
            Console.WriteLine();

            Console.WriteLine(orderable.Description);
            Console.WriteLine();

            Console.WriteLine("Meg szeretné rendelni? (Y/N)");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {
                Buy(identifier);
            }
        }

        private void SearchByName()
        {
            Console.WriteLine();
            Console.Write("Adja meg a keresett termék/szolgáltatás nevét: ");

            string identifier = Console.ReadLine();

            var orderable = _shop.Orderables.SearchByName(identifier);

            if (orderable == null)
            {
                Console.WriteLine("A megadott névvel nem található termék/szolgáltatás az adatbázisunkban!");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("A termék/szolgáltatás adatai:");
            Console.WriteLine();

            Console.WriteLine(orderable.Description);
            Console.WriteLine();

            Console.WriteLine("Meg szeretné rendelni? (Y/N)");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {
                Buy(identifier);
            }
        }

        private void GoToNextDay()
        {
            var auctions = _shop.GoToNextDay();

            Console.WriteLine();
            Console.WriteLine("Eltelt napok száma: " + _shop.Day);
            Console.WriteLine("Lezárt aukciók száma: " + auctions.Length);

            foreach (var auction in auctions)
            {
                Console.WriteLine("1.");
                Console.WriteLine("Termék neve: " + auction.Product.Name);

                Console.WriteLine("Sikeres megrendelések:");
                foreach (var order in auction.SuccessfulOrders)
                {
                    Console.WriteLine(order.Customer.Name + ": " + order.Count + " db");
                }

                Console.WriteLine("Sikertelen megrendelések: ");
                foreach (var order in auction.FailedOrders)
                {
                    Console.WriteLine(order.Customer.Name + ": " + order.Count + " db");
                }

                if (auction.CouldNotSellAll)
                {
                    Console.WriteLine("Nem sikerült eladni mindet!");
                }

                Console.WriteLine();
            }
        }

        private void Buy(string identifier)
        {
            IOrderable orderable = _shop.Orderables.BinarySearch(identifier);

            if (orderable == null)
            {
                Console.WriteLine("Rossz cikkszám!");
                return;
            }

            if (orderable is IOrderableWithCount)
            {
                Console.Write("Megrendelni kívánt darabszám: ");
                int count = int.Parse(Console.ReadLine());

                _shop.Order(_customer, orderable as IOrderableWithCount, count);
            }

            _shop.Order(_customer, orderable);
        }

        private void HandleSuccessfulOrder(Order order)
        {
            Console.WriteLine("Sikeres megrendelés!");
        }

        private void HandleFailedOrder(Order order)
        {
            Console.WriteLine("Megrendelés SIKERTELEN!!!");
        }
    }
}

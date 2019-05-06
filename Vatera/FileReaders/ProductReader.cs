using System.IO;
using Vatera.Models;

namespace Vatera.FileReaders
{
    class ProductReader
    {
        public static Product[] Read(string path)
        {
            string[] lines = File.ReadAllLines(path);

            int count = 0;
            Product[] products = new Product[(lines.Length + 1) / 6];

            string name;
            string identifier;
            int price;
            int quantity;
            int days;

            int linesCount = 0;
            while (linesCount < lines.Length)
            {
                name = lines[linesCount++];
                identifier = lines[linesCount++];
                price = int.Parse(lines[linesCount++]);
                quantity = int.Parse(lines[linesCount++]);
                days = int.Parse(lines[linesCount++]);

                products[count++] = new Product(name, identifier, price, quantity, days);

                linesCount++;
            }

            return products;
        }
    }
}

using System.IO;
using Vatera.Models;

namespace Vatera.FileReaders
{
    class CustomerReader
    {
        public static Customer[] Read(string path)
        {
            string[] lines = File.ReadAllLines(path);

            int count = 0;
            Customer[] customers = new Customer[(lines.Length + 1) / 4];

            string name;
            string identifier;
            int rating;

            int linesCount = 0;
            while (linesCount < lines.Length)
            {
                name = lines[linesCount++];
                identifier = lines[linesCount++];
                rating = int.Parse(lines[linesCount++]);

                customers[count++] = new Customer(name, identifier, rating);

                linesCount++;
            }

            return customers;
        }
    }
}

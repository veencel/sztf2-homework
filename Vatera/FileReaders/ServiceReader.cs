using System.IO;
using Vatera.Models;

namespace Vatera.FileReaders
{
    public class ServiceReader
    {
        public static Service[] Read(string path)
        {
            string[] lines = File.ReadAllLines(path);

            int count = 0;
            Service[] services = new Service[(lines.Length + 1) / 4];

            string name;
            string identifier;
            int price;

            int linesCount = 0;
            while (linesCount < lines.Length)
            {
                name = lines[linesCount++];
                identifier = lines[linesCount++];
                price = int.Parse(lines[linesCount++]);

                services[count++] = new Service(name, identifier, price);

                linesCount++;
            }

            return services;
        }
    }
}

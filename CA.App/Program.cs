using System;

namespace CA.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Parser.ParseClaimsData();

            Console.WriteLine($"{data.EarliestOriginYear} {data.TotalDevelopmentYears}");

            foreach (var product in data.Products)
            {
                var accumulatedValues = data.AccumulateValues(product.Name);
                Console.WriteLine($"{product.Name} " + String.Join(",", accumulatedValues));
            }
        }
    }
}

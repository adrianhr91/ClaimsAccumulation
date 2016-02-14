using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var accumulatedValues = data.AccumulateValues(product.ProductName);
                Console.WriteLine(String.Join(",", accumulatedValues));
            }
        }
    }
}

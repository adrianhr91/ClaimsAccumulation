using CA.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CA.App
{
    public class Parser
    {
        public static ClaimsData ParseClaimsData()
        {
            var reader = new StreamReader(File.OpenRead(@"../../../data.csv"));
            var claimsData = new ClaimsData();

            using (reader)
            {
                // skip title line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');

                    string productName = line[0];
                    int originYear = Int32.Parse(line[1]);
                    int developmentYear = Int32.Parse(line[2]);
                    decimal incrementalValue = Decimal.Parse(line[3]);

                    var claim = new Product.Claim(originYear, developmentYear, incrementalValue);
                    claimsData.AddDevelopment(productName, claim);
                }
            }

            return claimsData;
        }
    }
}

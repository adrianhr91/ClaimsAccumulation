using System;
using System.Collections.Generic;

namespace CA.Domain.Models
{
    public class Product
    {
        public string ProductName { get; private set; }
        public List<Claim> Claims { get; private set; }

        public Product(string productName)
        {
            ProductName = productName;
            Claims = new List<Claim>();
        }

        public class Claim
        {
            public int OriginYear { get; private set; }
            public int DevelopmentYear { get; private set; }
            public decimal IncrementalValue { get; private set; }

            public Claim(int originYear, int developmentYear, decimal incrementalValue)
            {
                OriginYear = originYear;
                DevelopmentYear = developmentYear;
                IncrementalValue = incrementalValue;
            }
        }
    }
}

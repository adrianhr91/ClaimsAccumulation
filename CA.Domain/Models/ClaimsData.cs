using System;
using System.Collections.Generic;
using System.Linq;

namespace CA.Domain.Models
{
    public class ClaimsData
    {
        public List<Product> Products { get; private set; }

        public ClaimsData()
        {
            Products = new List<Product>();
        }
        public int EarliestOriginYear
        {
            get
            {
                return Products
                    .Min(prod => prod.Claims
                        .Min(claim => claim.OriginYear));
            }
        }

        public int LatestDevelopmentYear
        {
            get
            {
                return Products
                    .Max(prod => prod.Claims
                        .Max(claim => claim.DevelopmentYear));
            }
        }

        public int TotalDevelopmentYears
        {
            get
            {
                return LatestDevelopmentYear - EarliestOriginYear + 1;
            }
        }

        public void AddDevelopment(string productName, Product.Claim claim)
        {

            if (Products.Any(prod => prod.Name == productName))
            {
                UpdateProduct(productName, claim);
            }
            else
            {
                AddProduct(productName, claim);
            }
        }

        private void UpdateProduct(string productName, Product.Claim claim)
        {
            var product = Products.Single(prod => prod.Name == productName);
            product.Claims.Add(claim);
        }

        private void AddProduct(string productName, Product.Claim claim)
        {
            var product = new Product(productName);
            product.Claims.Add(claim);

            Products.Add(product);
        }

        public List<decimal> AccumulateValues(string productName)
        {
            var values = new List<decimal>();
            var product = Products.FirstOrDefault(prod => prod.Name == productName);
            var claimsByOriginYear = GroupClaimsByOriginYear(product);

            // iterate origin years
            for (int originYear = EarliestOriginYear; originYear < EarliestOriginYear + TotalDevelopmentYears; originYear++)
            {
                var claimsForOriginYear = claimsByOriginYear
                    .FirstOrDefault(group => group.Key == originYear).Value;

                decimal accumulatedValue = 0;

                // iterate development years for origin year
                for (int developmentYear = originYear;
                    developmentYear < originYear + (LatestDevelopmentYear - originYear + 1);
                    developmentYear++)
                {
                    var claim = claimsForOriginYear
                        .FirstOrDefault(c => c.DevelopmentYear == developmentYear);

                    if (claim != null)
                    {
                        accumulatedValue += claim.IncrementalValue;
                    }

                    values.Add(accumulatedValue);
                }
            }

            return values;
        }

        private Dictionary<int, List<Product.Claim>> GroupClaimsByOriginYear(Product product)
        {
            var claimsByOriginYear = product.Claims
                    .GroupBy(prod => prod.OriginYear)
                    .ToDictionary(group => group.Key, group => group.ToList());

            // Generate missing years
            for (int originYear = EarliestOriginYear; originYear < LatestDevelopmentYear; originYear++)
            {
                bool hasClaimsForYear = claimsByOriginYear.Any(group => group.Key == originYear);

                if (!hasClaimsForYear)
                {
                    claimsByOriginYear.Add(originYear, new List<Product.Claim>());
                }
            }

            return claimsByOriginYear;
        }
    }
}

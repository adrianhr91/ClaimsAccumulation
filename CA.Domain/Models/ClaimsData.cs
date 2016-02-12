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

        public void AddDevelopment(string productName, Product.Claim claim)
        {

            if (Products.Any(prod => prod.ProductName == productName))
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
            var product = Products.Single(prod => prod.ProductName == productName);
            product.Claims.Add(claim);
        }

        private void AddProduct(string productName, Product.Claim claim)
        {
            var product = new Product(productName);
            product.Claims.Add(claim);

            Products.Add(product);
        }
    }
}

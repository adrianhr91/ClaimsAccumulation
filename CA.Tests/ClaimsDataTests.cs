using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CA.Domain.Models;
using System.Collections.Generic;

namespace CA.Tests
{
    [TestClass]
    public class ClaimsDataTests
    {
        [TestMethod]
        public void EarliestOriginYearTest()
        {
            // Arrange
            var data = new ClaimsData();
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod1", new Product.Claim(1998, 1998, 1));
            data.AddDevelopment("prod2", new Product.Claim(1990, 1991, 1));

            // Assert
            Assert.AreEqual(1990, data.EarliestOriginYear);
        }

        [TestMethod]
        public void LatestDevelopmentYearTest()
        {
            // Arrange
            var data = new ClaimsData();
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod1", new Product.Claim(1991, 1998, 1));
            data.AddDevelopment("prod2", new Product.Claim(1990, 1991, 1));

            // Assert
            Assert.AreEqual(1998, data.LatestDevelopmentYear);
        }

        [TestMethod]
        public void TotalDevelopmentYearsTest()
        {
            // Arrange
            var data = new ClaimsData();
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod2", new Product.Claim(1992, 1998, 1));

            // Assert
            Assert.AreEqual(8, data.TotalDevelopmentYears);
        }

        [TestMethod]
        public void AddDevelopment_NewProductTest()
        {
            // Arrange
            var data = new ClaimsData();
            var actualClaim = new Product.Claim(1991, 1991, 1);
            var actualProductName = "prod1";

            // Act
            data.AddDevelopment(actualProductName, actualClaim);

            // Assert
            Assert.AreEqual(1, data.Products.Count);
            Assert.AreEqual(actualProductName, data.Products[0].Name);
            Assert.AreEqual(1, data.Products[0].Claims.Count);
        }

        [TestMethod]
        public void AddDevelopment_ExistingProductTest()
        {
            // Arrange
            var data = new ClaimsData();
            var actualProductName = "prod1";

            // Act
            data.AddDevelopment(actualProductName, new Product.Claim(1991, 1991, 1));
            data.AddDevelopment(actualProductName, new Product.Claim(1991, 1992, 2));

            // Assert
            Assert.AreEqual(1, data.Products.Count);
            Assert.AreEqual(actualProductName, data.Products[0].Name);
            Assert.AreEqual(2, data.Products[0].Claims.Count);
        }

        [TestMethod]
        public void AddDevelopment_MultipleProductsTest()
        {
            // Arrange
            var data = new ClaimsData();

            // Act
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod2", new Product.Claim(1991, 1992, 2));

            // Assert
            Assert.AreEqual(2, data.Products.Count);
        }

        [TestMethod]
        public void AccumulateValues_MultipleProductsTest()
        {
            // Arrange
            var data = new ClaimsData();
            data.AddDevelopment("Travel", new Product.Claim(1992, 1992, 110));
            data.AddDevelopment("Travel", new Product.Claim(1992, 1993, 170));
            data.AddDevelopment("Travel", new Product.Claim(1993, 1993, 200));
            data.AddDevelopment("House", new Product.Claim(1990, 1990, 45.2m));
            data.AddDevelopment("House", new Product.Claim(1990, 1991, 64.8m));
            data.AddDevelopment("House", new Product.Claim(1990, 1993, 37.0m));
            data.AddDevelopment("House", new Product.Claim(1991, 1991, 50.0m));
            data.AddDevelopment("House", new Product.Claim(1991, 1992, 75.0m));
            data.AddDevelopment("House", new Product.Claim(1991, 1993, 25.0m));
            data.AddDevelopment("House", new Product.Claim(1992, 1992, 55.0m));
            data.AddDevelopment("House", new Product.Claim(1992, 1993, 85.0m));
            data.AddDevelopment("House", new Product.Claim(1993, 1993, 100.0m));
            var expectedTravelValues =
                String.Join(",", new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 110, 280, 200 });

            var expectedHouseValues =
                String.Join(",", new List<decimal> { 45.2m, 110.0m, 110.0m, 147.0m, 50.0m, 125.0m, 150.0m, 55.0m, 140.0m, 100.0m });

            // Act
            var travelValues = data.AccumulateValues("Travel");
            var houseValues = data.AccumulateValues("House");

            // Assert
            Assert.AreEqual(expectedTravelValues, String.Join(",", travelValues));
            Assert.AreEqual(expectedHouseValues, String.Join(",", houseValues));
        }

        [TestMethod]
        public void AccumulateValues_MissingOriginYearTest()
        {
            // Arrange
            var data = new ClaimsData();
            // 1991
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            data.AddDevelopment("prod1", new Product.Claim(1991, 1992, 2));
            data.AddDevelopment("prod1", new Product.Claim(1991, 1993, 3));
            // 1992 - missing
            // 1993
            data.AddDevelopment("prod1", new Product.Claim(1993, 1993, 4));

            var expectedValues = String.Join(",", new List<decimal> { 1, 3, 6, 0, 0, 4 });

            // Act
            var actualValues = data.AccumulateValues("prod1");

            // Assert
            Assert.AreEqual(expectedValues, String.Join(",", actualValues));
        }

        [TestMethod]
        public void AccumulateValues_IncrementalValuesMissingTest()
        {
            // Arrange
            var data = new ClaimsData();
            // 1991
            data.AddDevelopment("prod1", new Product.Claim(1991, 1991, 1));
            // 1991 - 1992 missing
            data.AddDevelopment("prod1", new Product.Claim(1991, 1993, 3));
            // 1992 - missing
            // 1993
            data.AddDevelopment("prod1", new Product.Claim(1993, 1993, 4));

            var expectedValues = String.Join(",", new List<decimal> { 1, 1, 4, 0, 0, 4 });

            // Act
            var actualValues = data.AccumulateValues("prod1");

            // Assert
            Assert.AreEqual(expectedValues, String.Join(",", actualValues));
        }
    }
}

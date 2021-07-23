using System;
using System.Collections.Generic;

namespace WebClient.Entities
{
    public class Store
    {
        public string storeId { get; set; }
        public List<Product> products { get; set; }

        public StoreSummary calcTotalMetrics()
        {
            var mostPopularItem = getMostPopularItem();
            var mostPopularValue = mostPopularItem.unitPrice * mostPopularItem.totalSold;
            return new StoreSummary
            {
                storeId = storeId,
                totalStockValue = Math.Round(calcStockValue(),2),
                totalSoldValue = Math.Round(calcSoldValue(),2),
                mostPopularItem = mostPopularItem.name,
                mostPopularItemSoldValue = Math.Round(mostPopularValue,2)
            };
        }
        public double calcSoldValue()
        {
            double value = 0;
            foreach (Product product in products)
            {
                value += product.totalSold * product.unitPrice;
            }
            return value;
        }
        public double calcStockValue()
        {
            double value = 0;
            foreach (Product product in products)
            {
                value += product.inStock * product.unitPrice;
            }
            return value;
        }
        public Product getMostPopularItem()
        {
            Product mostPopular = new Product();
            foreach(Product product in products)
            {
                if (product.totalSold > mostPopular.totalSold)
                {
                    mostPopular = product;
                }
            }
            return mostPopular;
        }

        public Product getProduct(string productNeedle)
        {
            return products.Find(p => p.name == productNeedle);
        }
    }
}

using System;
namespace WebClient.Entities
{
    public class StoreSummary
    {
        public string storeId { get; set; }
        public double totalStockValue { get; set; }
        public double totalSoldValue { get; set; }
        public string mostPopularItem { get; set; }
        public double mostPopularItemSoldValue { get; set; }
    }
}

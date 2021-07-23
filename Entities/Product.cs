using System;
namespace WebClient.Entities
{
    public class Product
    {
        public string name { get; set; }
        public double inStock { get; set; }
        public double unitPrice { get; set; }
        public double totalSold { get; set; }
    }
}

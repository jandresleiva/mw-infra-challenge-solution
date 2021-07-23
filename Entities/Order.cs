using System;
using System.Collections.Generic;

namespace WebClient.Entities
{
    public class Order
    {
        public string orderId { get; set; }
        public List<string> partnerStores { get; set; }
        public string productToBuy { get; set; }
        public double quantity { get; set; }
    }
}

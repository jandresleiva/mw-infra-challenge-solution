using System;
using System.Collections.Generic;

namespace WebClient.Entities
{
    public class PurchaseOrder
    {
        public string storeId { get; set; }
        public double quantity { get; set; }
        public double totalCost { get; set; }
    }
}

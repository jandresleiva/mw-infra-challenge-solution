using System;
using System.Collections.Generic;

namespace WebClient.Entities
{
    public class PurchaseRequest
    {
        public string orderId { get; set; }
        public List<PurchaseOrder> orders { get; set; }
        public List<StoreError> errors { get; set; }
        public double remainingQuantity { get; set; }
        public double totalCost { get; set; }
    }
}

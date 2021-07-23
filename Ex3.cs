using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebClient.Entities;

namespace WebClient
{
    public class Ex3
    {
        private readonly object locker = new object();
        private List<StoreError> errors = new List<StoreError>();
        private List<Store> stores = new List<Store>();

        private List<PurchaseOrder> purchaseList = new List<PurchaseOrder>();

        public async Task Run()
        {
            Console.WriteLine("Starting Ex3");
            // Get the order first
            try
            {
                var myOrder = await GetOrder();
                var productNeedle = myOrder.productToBuy;
                var productAmount = myOrder.quantity;
                var productAmountLeft = productAmount;
                double totalCost = 0;

                // Now get the listed stores
                List<Task> fetchStores = new List<Task>();
                foreach (string storeId in myOrder.partnerStores)
                {
                    var storeTask = GetStore(storeId);
                    fetchStores.Add(storeTask);
                    await Task.WhenAll(fetchStores);

                }

                /** Order stores by product price */
                List<Store> orderedStores = stores.OrderBy(p => {
                    if (p.getProduct(productNeedle) != null)
                    {
                        return p.getProduct(productNeedle).unitPrice;
                    }
                        return 0;
                    }
                ).ToList();

                /** From the ordered list, start picking product qtyes until you're full */
                /** TODO: This could yet be extracted to a method within PurchaseRequest for a better responsibility division*/
                foreach(Store store in orderedStores)
                {
                    var storeProduct = store.getProduct(productNeedle);
                    if (storeProduct != null)
                    {
                        double purchaseQty = 0;
                        if (productAmountLeft > 0)
                        {
                            purchaseQty = (productAmountLeft >= storeProduct.inStock) ? storeProduct.inStock : productAmountLeft;
                            productAmountLeft -= purchaseQty;
                            var purchaseTotalPrice = purchaseQty * storeProduct.unitPrice;
                            totalCost += purchaseTotalPrice;
                            purchaseList.Add(new PurchaseOrder { storeId = store.storeId, quantity = purchaseQty, totalCost = purchaseTotalPrice });
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                var purchaseRequest = new PurchaseRequest
                {
                    orderId = myOrder.orderId,
                    orders = purchaseList,
                    errors = errors,
                    remainingQuantity = productAmountLeft,
                    totalCost = totalCost
                };

                Console.WriteLine("Response: " + await PostResult(purchaseRequest) );

            }
            catch (Exception generalException)
            {
                Console.WriteLine("Error: " + generalException.Message);
                Console.WriteLine("Line: " + generalException.ToString());
            }

        }

        protected void ShowErrors()
        {
            foreach (StoreError error in errors)
            {
                Console.WriteLine(error);
            }
        }
        protected void ShowOrders()
        {
            foreach (PurchaseOrder order in purchaseList)
            {
                Console.WriteLine("store: " + order.storeId + "qty: " + order.quantity + "price: " + order.totalCost);
            }
        }
        protected void ShowStores(string productNeedle)
        {
            foreach (Store store in stores)
            {
                Console.WriteLine("store: ");
                //store.showStore();
                Console.WriteLine("Products: ");
                //store.showStores();

                var auxProduct = store.getProduct(productNeedle);
                if (auxProduct != null)
                {
                    //Console.WriteLine(auxProduct.getName());
                    //Console.WriteLine(auxProduct.getInStock());
                    //Console.WriteLine(auxProduct.getUnitPrice());
                }
            }
        }

        public async Task<Order> GetOrder()
        {
            var endpoint = "data-aggregation/start";
            var response = await Connection.GetQuery(endpoint);
            return JsonConvert.DeserializeObject<Order>(response);
        }

        public async Task GetStore(string storeId)
        {
            var endpoint = "data-aggregation/data/store/" + storeId;
            try
            {
                var response = await Connection.GetQuery(endpoint);
                var store = JsonConvert.DeserializeObject<Store>(response);

                lock (locker)
                {
                    stores.Add(store);
                    //Console.WriteLine(stores.Count);
                }
            }
            catch (Exception getStoreException)
            {
                Console.WriteLine("Error logged for " + storeId);
                errors.Add(new StoreError { storeId = storeId, message = getStoreException.Message });
            }
        }

        public async Task<string> PostResult(PurchaseRequest request)
        {
            var endpoint = "data-aggregation/finish";
            string jsonPayload = JsonConvert.SerializeObject(request);
            Console.WriteLine("Ex3 - Response");
            return await Connection.PostRequest(endpoint, jsonPayload);
        }
    }
}

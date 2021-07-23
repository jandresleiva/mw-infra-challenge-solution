using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebClient.Entities;

namespace WebClient
{
    public class Ex2
    {
        public async Task Run()
        {
            Console.WriteLine("Starting Ex2");
            var result = Task.WhenAll(GetStore()).GetAwaiter().GetResult();
            Store store = JsonConvert.DeserializeObject<Store>(result[0]);
            var responseObj = store.calcTotalMetrics();
            Console.WriteLine("Response: " + await PostResult(responseObj));
        }

        public static async Task<string> GetStore()
        {
            var endpoint = "data-modification/start";
            var response = await Connection.GetQuery(endpoint);
            return response;
        }

        public async Task<string> PostResult(StoreSummary responseObj)
        {
            var endpoint = "data-modification/finish";
            string jsonPayload = JsonConvert.SerializeObject(responseObj);
            Console.WriteLine("Ex2 - Response");
            return await Connection.PostRequest(endpoint, jsonPayload);
        }
    }
}

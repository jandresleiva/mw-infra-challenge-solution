using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class Connection
    {
        private static string base_url = "http://infra-challenge.sandbox.madwire.network/wqnbcujv/";
        private static HttpClient client = new HttpClient();

        public static void setUp(string MediaType = "application/json")
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaType)
            );
            client.DefaultRequestHeaders.Add("User-Agent", "Andres C# Programm");
        }

        public static async Task<string> GetQuery(string endpoint)
        {
            var httpResponse = await client.GetAsync(base_url + endpoint);
            //Console.WriteLine("Getting from: " + base_url + endpoint);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            throw new Exception(httpResponse.ReasonPhrase);
        }

        public static async Task<string> PostRequest(string endpoint, string jsonPayload)
        {
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //Console.WriteLine("Posting to: " + base_url + endpoint);
            var response = await client.PostAsync(base_url + endpoint, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}

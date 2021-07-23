using System;
using System.Threading.Tasks;

namespace WebClient
{
    public class Ex1
    {
        public async Task Run()
        {
            Console.WriteLine("Starting Ex1");
            var delay = await RequestDelay();

            var parts = delay.Split(',');
            await Task.Delay(Int32.Parse(parts[0]) * 1000);
            await SecondRequest(parts[1]);
        }

        public async Task<string> RequestDelay()
        {
            var endpoint = "sleep/start";
            var response = await Connection.GetQuery(endpoint);
            return response;
        }

        public async Task SecondRequest(string endpointIdentifier)
        {
            Console.WriteLine("Ex1 - SecondRequest \n" + endpointIdentifier);
            var result2 = await Connection.GetQuery("sleep/finish/" + endpointIdentifier);
        }
    }
}

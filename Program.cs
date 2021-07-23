using System;
using System.Threading.Tasks;

namespace WebClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Connection.setUp();

            var ex1 = new Ex1();
            var ex2 = new Ex2();
            var ex3 = new Ex3();

            Task[] tasks =
            {
                ex1.Run(),
                ex2.Run(),
                ex3.Run()
            };

            await Task.WhenAll(tasks);

            Console.ReadKey();
        }
    }
}

using DepsTest.Models;
using DepsTest.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsTest
{
    class Program
    {
        private static HttpClient httpClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine(" Currency exchange test app.\n Emilia Voronova\n");
            try
            {
                var connection = JsonSerializer.Deserialize<CustomUri>(await File.ReadAllTextAsync("connectionOptions.json"));
                httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(connection.Uri)
                };
                await AuthenticationTestService.Registration(httpClient, new Account("string", "string"), 200);

                await AuthenticationTestService.Registration(httpClient, new Account("str", "str"), 400);

                await AuthenticationTestService.Registration(httpClient, new Account("string", "stringggg"), 400);

                await ExchangeTestService.GetAmountInAnotherCurrency(httpClient, "Rates/EUR/USD?amount=100", 401);

                await ExchangeTestService.GetAmountInAnotherCurrencyLogined("Rates/EUR/USD?amount=100", 200);
            }
            catch (Exception)
            {
                Console.WriteLine(" Oops! Something went wrong...");
            }
        }
    }
}

using DepsTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsTest.Services
{
    public static class ExchangeTestService
    {
        public static async Task GetAmountInAnotherCurrency(HttpClient client, string request, int expectedCode)
        {
            try
            {
                var uri = client.BaseAddress + request;
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                var response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"\n Request              : {request}");
                Console.WriteLine($" Expected Status Code : {expectedCode}");
                Console.WriteLine($" Actual Status Code   : {(int)response.StatusCode}");
                Console.WriteLine($" Response             : {content} ");
                WriteResult(expectedCode == (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oops! Something went wrong\n {ex.Message}");
            }
        }

        public static void WriteResult(bool isSucceeded)
        {
            Console.ForegroundColor = isSucceeded ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isSucceeded ? " SUCCEEDED" : " FAILED");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static async Task GetAmountInAnotherCurrencyLogined(string request, int expectedCode)
        {
            HttpClient _client;
            try
            {
                var json = JsonSerializer.Deserialize<CustomUri>(await File.ReadAllTextAsync("connectionOptions.json"));
                _client = new HttpClient()
                {
                    BaseAddress = new Uri(json.Uri)
                };
                var uri = _client.BaseAddress + request;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "c3RyaW5nZ2dnOnN0cmluZw==");
                var response = await _client.GetAsync(uri);
                string content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"\n Request              : {request}");
                Console.WriteLine($" Expected Status Code : {expectedCode}");
                Console.WriteLine($" Actual Status Code   : {(int)response.StatusCode}");
                Console.WriteLine($" Response             : {content} ");
                WriteResult(expectedCode == (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oops! Something went wrong...\n {ex.Message}");
            }
        }
    }
}

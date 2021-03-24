using DepsTest.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DepsTest.Services
{
    public static class AuthenticationTestService
    {
        public static async Task Registration(HttpClient client, Account account, int statusCodeExpected)
        {
            try
            {
                var request = client.BaseAddress + "Auth/register";
                var response = await client.PostAsync(request, account, new JsonMediaTypeFormatter());
                var content = await response.Content.ReadAsStringAsync();

                if (content.Split(" ")[0] != "Basic")
                    response.StatusCode = HttpStatusCode.BadRequest;

                ConsoleWriteResponse(request, $" Login : {account.Login} Password : {account.Password}", statusCodeExpected, (int)response.StatusCode, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Oops! Something went wrong... {ex.Message}");
            }
        }
        public static void ConsoleWriteResponse(string request, string body, int expectedStatusCode, int actualStatusCode, string content)
        {
            Console.WriteLine($"\n Request              : {request}");
            if (body != null) Console.WriteLine($"{body}");
            Console.WriteLine($" Expected Status Code : {expectedStatusCode}");
            Console.WriteLine($" Actual Status Code   : {actualStatusCode}");
            Console.WriteLine($" Response             : {content} ");

            WriteResult(expectedStatusCode == actualStatusCode);
        }

        public static void WriteResult(bool isSucceeded)
        {
            Console.ForegroundColor = isSucceeded ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isSucceeded ? " SUCCEEDED" : " FAILED");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}

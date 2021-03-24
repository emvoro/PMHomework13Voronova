using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepsWebApp.Authentication
{
#pragma warning disable CS1591
    public static class Base64Encryption
    {
        public static string Encode(string account)
        {
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(account));
        }

        public static string Decode(string account)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(account.ToString().Split("Basic ")[1]));
        }
    }
#pragma warning restore CS1591
}

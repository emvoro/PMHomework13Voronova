using DepsWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DepsWebApp.Authentication
{

#pragma warning disable CS1591
    public class CustomAuthSchemaHandler : AuthenticationHandler<CustomAuthSchemaOptions>
    {
        private readonly IAuthService _authService;

        public CustomAuthSchemaHandler(IOptionsMonitor<CustomAuthSchemaOptions> options,
           ILoggerFactory logger,
           UrlEncoder encoder,
           ISystemClock clock,
           IAuthService accountCoordinator) : base(options, logger, encoder, clock)
        {
            _authService = accountCoordinator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!DecryptAccount(Request, out var account)) return AuthenticateResult.NoResult();

            try
            {
                if (await _authService.GetUserAccount(account))
                {
                    var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, account) };
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, "ApplicationCookie",
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType)), CustomAuthSchema.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else throw new AuthenticationException("Invalid credentials.");
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid credentials.");
            }
        }

        private bool DecryptAccount(HttpRequest request, out string account)
        {
            account = null;

            if (request.Headers.ContainsKey(HeaderNames.Authorization))
                account = request.Headers[HeaderNames.Authorization].FirstOrDefault(); ;

            return !string.IsNullOrEmpty(account);
        }
    }
#pragma warning restore CS1591
}

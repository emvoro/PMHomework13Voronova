using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DepsWebApp.Filters;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DepsWebApp.Contracts;
using System.Net;
using DepsWebApp.Services;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// This controller is used for account operations(authorization, authentication...)
    /// </summary>
    [ApiController]
    [CustomExceptionFilter]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthService _authService;

#pragma warning disable CS1591 
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
#pragma warning restore CS1591

        /// <summary>
        /// Method that registers an incoming account on the platform
        /// </summary>
        /// <param name="account">Account model that will be registered in api</param>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> Register([FromBody] AccountDTO account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestResult = await _authService.RegisterAsync(account.Login, account.Password);
            _logger.LogInformation($"User {account.Login} registered successfully.");

            return Ok(requestResult);
        }
    }
}

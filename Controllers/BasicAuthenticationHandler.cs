using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace dotnet_resources_api
{   
    public class User
    {
        public string username { get; set; }

        public string password { get; set; }
    }
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // A hard-coded list of users for demo purposes
        private readonly List<User> _users = new List<User>
        {
            new User { username = "admin", password = "dotnet_resources_api" },
        };

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Skip authentication if request does not contain Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            User user = null;

            try
            {
                // Extract username and password from Authorization header
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                // Validate username and password against user store
                user = _users.FirstOrDefault(u => u.username == username && u.password == password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            // Return failure result if user is not found or credentials are invalid
            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            // Create user identity and claims
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return success result with the user ticket
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Set 401 status code and WWW-Authenticate header with scheme and realm name
            Response.StatusCode = 401;
            Response.Headers.Append("WWW-Authenticate", $"Basic realm=0");

            // Write "Unauthorized" to response body
            await Response.WriteAsync("Unauthorized");
        }
    }
}

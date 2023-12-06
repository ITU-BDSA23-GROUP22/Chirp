using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace Chirp.Web.Test
{
    public class TestAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
    }

    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationHandlerOptions>
    {
        public const string UserName = "UserName";
        public const string UserEmail = "UserEmail";

        public const string AuthenticationScheme = "Test";

        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var hasUserName = Context.Request.Headers.TryGetValue(TestAuthenticationHandler.UserName, out var userName);
            var hasUserEmail = Context.Request.Headers.TryGetValue(TestAuthenticationHandler.UserEmail, out var userEmail);

            if (hasUserName && hasUserEmail)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName[0]),
                    new Claim("emails", userEmail[0]),
                };

                var identity = new ClaimsIdentity(claims, AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }

            return Task.FromResult(AuthenticateResult.Fail("Access Denied"));
        }
    }
}


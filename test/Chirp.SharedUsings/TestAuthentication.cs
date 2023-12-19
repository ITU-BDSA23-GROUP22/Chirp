using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chirp.SharedUsings
{ 

    public class TestAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
    }

    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationHandlerOptions>
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";

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

            var hasUserId = Context.Request.Headers.TryGetValue(TestAuthenticationHandler.UserId, out var userId);
            var hasUserName = Context.Request.Headers.TryGetValue(TestAuthenticationHandler.UserName, out var userName);


            if (hasUserId && hasUserName)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName[0] ?? throw new Exception($"Failed to get username")),
                    new Claim(ClaimTypes.NameIdentifier, userId[0] ?? throw new Exception($"Failed to get userid")),
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


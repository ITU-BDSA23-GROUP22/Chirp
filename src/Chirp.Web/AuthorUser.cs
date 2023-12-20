// ReferenceLink:
//  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-7.0
//  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio
//  https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal?view=net-7.0

using System.Security.Claims;
using System.Security.Principal;
using Chirp.Core;

namespace Chirp.Web
{
    /// <summary>
    ///     Provides a wrapper for authenticated Authors.
    ///     EnsureAuthorCreatedFilter will use this wrapper, to wrap authenticated user with AuthorUser.
    ///     Markup code and PageModels can access AuthorUser directly on HttpContext or PageModel
    /// </summary>
    public class AuthorUser : ClaimsPrincipal
    {
        public Guid AuthorId { get; private set; }
        public string AuthorName { get; private set; }
        public IEnumerable<Guid> FollowedAuthorIds { get; private set; }

        public AuthorUser(IPrincipal user, AuthorDTO authorDto) : base(user)
        {
            this.AuthorId = authorDto.Id;
            this.AuthorName = authorDto.Name;
            this.FollowedAuthorIds = authorDto.followingIds;
        }
    }
}


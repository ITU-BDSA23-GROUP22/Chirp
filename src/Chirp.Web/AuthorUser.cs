using System.Security.Claims;
using System.Security.Principal;
using Chirp.Core;

namespace Chirp.Web
{
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


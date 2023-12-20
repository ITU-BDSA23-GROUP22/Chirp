// ReferenceLink:
//  https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chirp.Web.Filters
{
    /// <summary>
    ///     Razor Authentication Filter to ensure that an authenticated user is always
    ///     created as an author in the database    
    /// </summary>
    public class EnsureAuthorCreatedFilter : IAsyncAuthorizationFilter
    {
        private readonly IPresentationService presentationService;

        public EnsureAuthorCreatedFilter(IPresentationService presentationService)
        {
            this.presentationService = presentationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var hasPage = context.ActionDescriptor.RouteValues.TryGetValue("page", out var page);
            var hasArea = context.ActionDescriptor.RouteValues.TryGetValue("area", out var area);

            if (hasPage && page == "/SignIn" || hasArea && area == "MicrosoftIdentity")
            {
                // For sign-in/out ignore creating author
                return;
            }

            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // For all authenticated requests, ensure that author is created

                var authorName = user.Identity.Name;
                var authorIdString = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (authorName != null && Guid.TryParse(authorIdString, out var authorId))
                {
                    // Get existing or Create new Author..
                    var authorDto = await this.presentationService.GetOrCreateAuthor(authorId, authorName);

                    if (authorDto != null)
                    {
                        // override user with wrapped AuthorUser
                        context.HttpContext.User = new AuthorUser(user, authorDto);
                    }
                }
            }
        }
    }
}


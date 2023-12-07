using System.Security.Claims;
using Chirp.Core;
using Chirp.Core.Services;
using Chirp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Chirp.Web
{
    public class HelperService : IHelperService
    {
        public const int MAX_CHEEPS_PER_PAGE = 5;

        private readonly IChirpService chirpService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal? user;

        public HelperService(IHttpContextAccessor httpContextAccessor, IChirpService chirpService)
        {
            this.chirpService = chirpService
                ?? throw new ArgumentNullException(nameof(chirpService));

            this.httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            this.user = this.httpContextAccessor.HttpContext?.User;
        }

        public async Task<Guid> GetAuthorId()
        {
            var email = this.httpContextAccessor.HttpContext?.User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty;
            var name = this.httpContextAccessor.HttpContext?.User.Claims?.SingleOrDefault(x => x.Type == "name")?.Value ?? string.Empty;

            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to CreateCheep - No authenticated author");

            var authorDto = await this.chirpService.GetAuthor(authenticatedAuthorDto);

            if (authorDto == null)
            {

                // return Guid.Empty;
            }

            return authorDto.Id;
        }

        public async Task<CheepListViewModel> GetAllCheepsViewModel(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(nameof(pageNumber));
            }

            var authorDto = await this.GetAuthor();

            var cheepDtos = await this.chirpService.GetAllCheeps(pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);

            return new CheepListViewModel(authorDto, cheepDtos, pageNumber, MAX_CHEEPS_PER_PAGE, "/");
        }

        public async Task<CheepListViewModel> GetCheepsByAuthorsViewModel(IEnumerable<Guid> authorIds, int pageNumber, string pageUrl)
        {
            var authenticatedAuthorDto = await this.GetAuthor();

            var cheepDtos = await this.chirpService.GetCheepsByAuthors(authorIds, pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);

            return new CheepListViewModel(authenticatedAuthorDto, cheepDtos, pageNumber, MAX_CHEEPS_PER_PAGE, pageUrl);
        }

        public async Task CreateCheep(string cheepText)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to CreateCheep - No authenticated author");

            await this.chirpService.CreateCheep(authenticatedAuthorDto, cheepText);
        }

        public async Task FollowAuthor(Guid authorToFollowId)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor();
            // ?? throw new Exception("Failed to FollowAuthor - No authenticated author");

            await this.chirpService.FollowAuthor(authenticatedAuthorDto!, authorToFollowId);
        }

        public async Task UnfollowAuthor(Guid authorToFollowId)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to UnfollowAuthor - No authenticated author");

            await this.chirpService.UnfollowAuthor(authenticatedAuthorDto, authorToFollowId);
        }

        public async Task<AuthorDTO?> GetAuthor(Guid authorId)
        {

            var author = await this.chirpService.GetAuthor(authorId);
            // ?? throw new Exception("Failed GetAuther - Author not found");
            if (author == null)
            {
                var authenticatedAuthorDto = this.GetAuthenticatedAuthor();
                author = await this.chirpService.GetAuthor(authenticatedAuthorDto!);
            }

            return author;
        }

        public async Task<AuthorDTO?> GetAuthor()
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor();
            if (authenticatedAuthorDto == null)
            {
                // Anonymous access..
                return null;
            }

            var author = await this.chirpService.GetAuthor(authenticatedAuthorDto);

            if (author == null)
            {
                // Authenticated author has not been create in database..
                // Author is only created in database when creating cheeps, follow or unfollow
                author = new AuthorDTO(Guid.Empty, authenticatedAuthorDto.Name, authenticatedAuthorDto.Email, Enumerable.Empty<Guid>());
            }

            return author;
        }

        private AuthenticatedAuthorDTO? GetAuthenticatedAuthor()
        {
            if (this.user?.Identity?.IsAuthenticated == true)
            {
                var userName = this.user.Identity?.Name;
                var userEmail = this.user.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value;

                if (userName != null && userEmail != null)
                {
                    var authenticatedAuthorDto = new AuthenticatedAuthorDTO(userName, userEmail);
                    return authenticatedAuthorDto;
                    // return await this.chirpService.GetAuthor(authenticatedAuthorDto);
                }
            }
            return null;
        }
    }
}
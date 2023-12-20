using System.Security.Claims;
using System.Text;
using Chirp.Core;
using Chirp.Core.Services;
using Chirp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web
{
    /// <inheritdoc/>
    public class PresentationService : IPresentationService
    {
        public const int MAX_DOWNLOAD_CHEEPS = 1000;
        public const int MAX_CHEEPS_PER_PAGE = 5;
        public const int MAX_AUTHORS_PER_PAGE = 5;

        private readonly IChirpService chirpService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal? user;

        public PresentationService(IHttpContextAccessor httpContextAccessor, IChirpService chirpService)
        {
            this.chirpService = chirpService
                ?? throw new ArgumentNullException(nameof(chirpService));

            this.httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            this.user = this.httpContextAccessor.HttpContext?.User;
        }

        /// <inheritdoc/>
        public AuthorDTO? GetAuthenticatedAuthor()
        {
            var user = this.httpContextAccessor.HttpContext?.User;

            var authenticatedAuthorUser = user as AuthorUser;

            if (authenticatedAuthorUser == null)
            {
                // Anonymous access..
                return null;
            }

            // authenticated AuthorUser
            return new AuthorDTO(
                authenticatedAuthorUser.AuthorId,
                authenticatedAuthorUser.AuthorName,
                authenticatedAuthorUser.FollowedAuthorIds
                );
        }

        /// <inheritdoc/>
        public async Task<CheepListViewModel> GetAllCheepsViewModel(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(nameof(pageNumber));
            }

            var authorDto = this.GetAuthenticatedAuthor();

            var cheepDtos = await this.chirpService.GetAllCheeps(pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);

            //IF REQUESTED PAGE HAS NO CHEEPS AND IS NOT FIRST PAGE, DEFAULT TO PAGE 1
            if (!cheepDtos.Any() && pageNumber > 1)
            {
                pageNumber = 1;
                cheepDtos = await this.chirpService.GetAllCheeps(pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);
            }

            return new CheepListViewModel(authorDto, cheepDtos, pageNumber, MAX_CHEEPS_PER_PAGE, "/");
        }

        /// <inheritdoc/>
        public async Task<CheepListViewModel> GetCheepsByAuthorsViewModel(IEnumerable<Guid> authorIds, int pageNumber, string pageUrl)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(nameof(pageNumber));
            }

            var authenticatedAuthorDto = this.GetAuthenticatedAuthor();

            var cheepDtos = await this.chirpService.GetCheepsByAuthors(authorIds, pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);

            //IF REQUESTED PAGE HAS NO CHEEPS AND IS NOT FIRST PAGE, DEFAULT TO PAGE 1
            if (!cheepDtos.Any() && pageNumber > 1)
            {
                pageNumber = 1;
                cheepDtos = await this.chirpService.GetCheepsByAuthors(authorIds, pageNumber, MAX_CHEEPS_PER_PAGE * (pageNumber - 1), MAX_CHEEPS_PER_PAGE + 1);
            }

            return new CheepListViewModel(authenticatedAuthorDto, cheepDtos, pageNumber, MAX_CHEEPS_PER_PAGE, pageUrl);
        }

        /// <inheritdoc/>
        public async Task CreateCheep(string cheepText)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to CreateCheep - No authenticated author");

            await this.chirpService.CreateCheep(authenticatedAuthorDto.Id, cheepText);
        }

        /// <inheritdoc/>
        public async Task FollowAuthor(Guid authorToFollowId)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to FollowAuthor - No authenticated author");

            await this.chirpService.FollowAuthor(authenticatedAuthorDto.Id, authorToFollowId);
        }

        /// <inheritdoc/>
        public async Task UnfollowAuthor(Guid authorToFollowId)
        {
            var authenticatedAuthorDto = this.GetAuthenticatedAuthor()
                ?? throw new Exception("Failed to UnfollowAuthor - No authenticated author");

            await this.chirpService.UnfollowAuthor(authenticatedAuthorDto.Id, authorToFollowId);
        }

        /// <inheritdoc/>
        public async Task<AuthorDTO> GetAuthor(Guid authorId)
        {
            var author = await this.chirpService.GetAuthor(authorId)
                ?? throw new Exception("Failed GetAuther - Author not found");

            return author;
        }

        /// <inheritdoc/>
        public async Task<AuthorDTO> GetOrCreateAuthor(Guid authorId, string authorName)
        {
            var author = await this.chirpService.GetAuthor(authorId);

            if (author == null)
            {
                author = await this.chirpService.CreateAuthor(authorId, authorName)
                    ?? throw new Exception("Faield GetOrCreateAuthor - Author not created");
            }

            return author;
        }

        /// <inheritdoc/>
        public async Task<AuthorListViewModel> GetAuthorListViewModel(string? searchText, int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(nameof(pageNumber));
            }

            var authorDto = this.GetAuthenticatedAuthor();

            var authorDtos = await this.chirpService.SearchAuthors(searchText, pageNumber, MAX_AUTHORS_PER_PAGE * (pageNumber - 1), MAX_AUTHORS_PER_PAGE + 1);

            //IF REQUESTED PAGE HAS NO CHEEPS AND IS NOT FIRST PAGE, DEFAULT TO PAGE 1
            if (!authorDtos.Any() && pageNumber > 1)
            {
                pageNumber = 1;
                authorDtos = await this.chirpService.SearchAuthors(searchText, pageNumber, MAX_AUTHORS_PER_PAGE * (pageNumber - 1), MAX_AUTHORS_PER_PAGE + 1);
            }

            return new AuthorListViewModel(authorDto, authorDtos, pageNumber, MAX_AUTHORS_PER_PAGE, "/Authors", searchText);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AuthorDTO>> GetFollowingAuthors(Guid authorId)
        {
            var author = await chirpService.GetAuthor(authorId)
                ?? throw new Exception("GetFollowingAuthors - Author not found");

            return await this.chirpService.GetAuthors(author.followingIds);
        }

        /// <inheritdoc/>
        public async Task AnonymizeAuthor(Guid authorId)
        {
            await this.chirpService.AnonymizeAuthor(authorId);
        }

        /// <inheritdoc/>
        public async Task<string> GetCheepsAndFollowerDownloadForAuthor(Guid authorId)
        {
            var author = await this.chirpService.GetAuthor(authorId);

            var sb = new StringBuilder();

            sb.AppendLine("Name:");
            sb.AppendLine(author.Name);
            sb.AppendLine();
            sb.AppendLine("Followed users:");

            var authors = await chirpService.GetAuthors(author.followingIds);

            foreach (var followedAuthor in authors)
            {
                sb.AppendLine($"{followedAuthor.Name}");
            }

            sb.AppendLine();
            sb.AppendLine("Cheeps:");

            var cheeps = await chirpService.GetCheepsByAuthors(new[] { author.Id }, 1, 0, MAX_DOWNLOAD_CHEEPS);

            foreach (var cheep in cheeps)
            {
                sb.AppendLine($"'{cheep.Message}' at {cheep.Timestamp}");
            }

            return sb.ToString();
        }
    }
}
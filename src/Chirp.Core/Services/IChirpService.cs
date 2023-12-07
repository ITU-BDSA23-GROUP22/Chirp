namespace Chirp.Core.Services
{
        public interface IChirpService
        {
                Task<AuthorDTO?> GetAuthor(Guid authorId);

                Task<AuthorDTO?> GetAuthor(AuthenticatedAuthorDTO authenticatedAuthorDto);

                Task<IEnumerable<CheepDTO>> GetAllCheeps(int page, int skipCount, int takeCount);

                Task<IEnumerable<CheepDTO>> GetCheepsByAuthors(IEnumerable<Guid> authorIds, int page, int skipCount, int takeCount);

                Task CreateCheep(AuthenticatedAuthorDTO authenticatedAuthorDto, string text);

                Task FollowAuthor(AuthenticatedAuthorDTO authenticatedAuthorDto, Guid authorToFollowId);

                Task UnfollowAuthor(AuthenticatedAuthorDTO authenticatedAuthorDto, Guid authorToUnfollowId);
        }
}
namespace Chirp.Core.Services
{
        public interface IChirpService
        {
                Task<AuthorDTO?> GetAuthor(Guid authorId);

                Task<AuthorDTO> CreateAuthor(Guid authorId, string authorName);

                Task<IEnumerable<CheepDTO>> GetCheepsByAuthors(IEnumerable<Guid> authorIds, int page, int skipCount, int takeCount);

                Task<IEnumerable<CheepDTO>> GetAllCheeps(int page, int skipCount, int takeCount);

                Task CreateCheep(Guid authorId, string text);

                Task FollowAuthor(Guid authorId, Guid authorToFollowId);

                Task UnfollowAuthor(Guid authorId, Guid authorToUnfollowId);

                Task<IEnumerable<AuthorDTO>> SearchAuthors(string? searchText, int page, int skipCount, int takeCount);
        }
}
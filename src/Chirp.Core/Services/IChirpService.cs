namespace Chirp.Core.Services
{
    public interface IChirpService
    {
        public Task<AuthorDTO> GetAuthor(Guid authorId);

        public Task<AuthorDTO?> GetAuthor(string authorEmail);

        public Task CreateCheep(AuthorDTO authorDto, string text);

        public Task<IEnumerable<CheepDTO>> GetAllCheeps(int page);

        public Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(Guid authorId, int page);

        public Task FollowAuthor(AuthorDTO authorDto, Guid authorToFollowId);

        public Task UnfollowAuthor(AuthorDTO authorDto, Guid authorToFollowId);

    }
}
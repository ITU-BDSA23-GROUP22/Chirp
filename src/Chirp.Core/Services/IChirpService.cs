namespace Chirp.Core.Services
{
    public interface IChirpService
    {
        public Task<AuthorDTO> GetAuthor(Guid authorId);

        public Task CreateCheep(AuthorDTO authorDto, string text);

        public Task<IEnumerable<CheepDTO>> GetAllCheeps(int page);

        public Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(Guid authorId, int page);

        public Task AddFollowing(Guid AuthorId, Guid followingId);

        public Task<IEnumerable<AuthorDTO>> GetFollowingByAuthor(Guid authorId);
    }
}
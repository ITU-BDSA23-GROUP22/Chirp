namespace Chirp.Core.Services
{
    public interface IChirpService
    {
        public Task<AuthorDTO> GetAuthor(int authorId);

        public Task CreateCheep(AuthorDTO authorDto, string text);

        public Task<IEnumerable<CheepDTO>> GetAllCheeps(int page);

        public Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(Guid authorId, int page);
    }
}
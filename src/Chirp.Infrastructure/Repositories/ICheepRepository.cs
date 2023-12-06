namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        public Task<Cheep> Create(Author author, string text, DateTime timestamp);
        public Task<IEnumerable<Cheep>> GetAll(int page);
        public Task<IEnumerable<Cheep>> GetByAuthor(Author author, int page);

    }
}
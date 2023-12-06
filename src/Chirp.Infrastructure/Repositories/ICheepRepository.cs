namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        Task<Cheep> Create(Author author, string text, DateTime timestamp);
        Task<IEnumerable<Cheep>> GetAll(int page);
        Task<IEnumerable<Cheep>> GetByAuthor(Author author, int page);

    }
}
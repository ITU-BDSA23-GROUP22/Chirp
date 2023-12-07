namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        Task<Cheep> Create(Author author, string text, DateTime timestamp);
        Task<IEnumerable<Cheep>> GetAll(int page, int skipCount, int takeCount);

        Task<IEnumerable<Cheep>> GetByAuthors(IEnumerable<Guid> authors, int page, int skipCount, int takeCount);


    }
}
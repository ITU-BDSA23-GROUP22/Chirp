namespace Chirp.Infrastructure
{
    public interface IAuthorRepository
    {
        Task<Author> Create(string name, string email);

        Task<Author?> Get(Guid authorId);

        Task<Author?> Get(string email);
    }
}
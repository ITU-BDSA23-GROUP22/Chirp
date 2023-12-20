namespace Chirp.Infrastructure
{
    public interface IAuthorRepository
    {
        Task<Author> Create(Guid authorId, string name);

        Task<Author?> Get(Guid authorId);

        Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow, DateTime timestamp);

        Task UnfollowAuthor(Author author, Author authorToUnfollow);

        Task DeleteAuthor(Author author);

        Task<IEnumerable<Author>> SearchAuthor(string? searchText, int page, int skipCount, int takeCount);
    }
}
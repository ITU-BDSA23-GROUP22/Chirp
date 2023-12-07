namespace Chirp.Infrastructure
{
    public interface IAuthorRepository
    {
        Task<Author> Create(string name, string email);

        Task<Author?> Get(Guid authorId);

        Task<Author?> Get(string? name, string email);

        Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow, DateTime timestamp);

        Task UnfollowAuthor(Author author, Author authorToUnfollow);

    }
}
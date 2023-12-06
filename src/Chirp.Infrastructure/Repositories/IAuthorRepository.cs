namespace Chirp.Infrastructure
{
    public interface IAuthorRepository
    {
        Task<Author> Create(string name, string email);

        Task<Author?> Get(Guid authorId);

        Task<Author?> Get(string email);

        Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow);

        Task UnfollowAuthor(Author author, Author authorToUnfollow);

        //Task<IEnumerable<Author?>> GetFollowing(Author author);
    }
}
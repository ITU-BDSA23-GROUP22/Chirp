namespace Chirp.Infrastructure
{
    /// <summary>
    ///     Provides repository access to database
    /// </summary>
    public interface IAuthorRepository
    {
        /// <summary>
        ///     Creates an Author in the database
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        Task<Author> Create(Guid authorId, string name);

        /// <summary>
        ///     Fetches a specified Author
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Author?> Get(Guid authorId);

        /// <summary>
        ///     Creates a new Follow Relation between two specified Authors in the database
        /// </summary>
        /// <param name="author"></param>
        /// <param name="authorToFollow"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow, DateTime timestamp);

        /// <summary>
        ///     Deletes a Follow Relation between two specified Authors in the database
        /// </summary>
        /// <param name="author"></param>
        /// <param name="authorToUnFollow"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        Task UnfollowAuthor(Author author, Author authorToUnfollow);

    }
}
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

        /// <summary>
        ///     Deletes specified Author from database
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        Task DeleteAuthor(Author author);

        /// <summary>
        ///     Fetches Authors based on specified search text and page
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        Task<IEnumerable<Author>> SearchAuthor(string? searchText, int page, int skipCount, int takeCount);
    }
}
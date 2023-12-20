namespace Chirp.Core.Services
{
    /// <summary>
    ///     Provides domain logic for Chirp! functionality
    /// </summary>
    public interface IChirpService
    {
        /// <summary>
        ///     Gets specified AuthorDTO from AuthorRepository
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<AuthorDTO?> GetAuthor(Guid authorId);

        /// <summary>
        ///     Creates specified Author in AuthorRepository
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="authorName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<AuthorDTO> CreateAuthor(Guid authorId, string authorName);

        /// <summary>
        ///     Gets Cheeps from specified Authors from CheepRepository
        /// </summary>
        /// <param name="authorIds"></param>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        Task<IEnumerable<CheepDTO>> GetCheepsByAuthors(IEnumerable<Guid> authorIds, int page, int skipCount, int takeCount);

        /// <summary>
        ///     Gets all Cheeps for a specified page from CheepRepository
        /// </summary>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<IEnumerable<CheepDTO>> GetAllCheeps(int page, int skipCount, int takeCount);

        /// <summary>
        ///     Creates specified Cheep in CheepRepository
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        Task CreateCheep(Guid authorId, string text);

        /// <summary>
        ///     Adds a Follow Relation between two specified Authors in AuthorRepository 
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="authorToFollowId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task FollowAuthor(Guid authorId, Guid authorToFollowId);


        /// <summary>
        ///     Removes a Follow Relation between two specified Authors in AuthorRepository
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="authorToUnfollowId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task UnfollowAuthor(Guid authorId, Guid authorToUnfollowId);

        /// <summary>
        ///     Anonymizes specified Author and removes any Follower Relation with the specified Author from AuthorRepository
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task AnonymizeAuthor(Guid authorId);

        /// <summary>
        ///     Gets a list of AuthorDTOs based on specified search text and page from AuhtorRepository
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        Task<IEnumerable<AuthorDTO>> SearchAuthors(string? searchText, int page, int skipCount, int takeCount);
    }
}
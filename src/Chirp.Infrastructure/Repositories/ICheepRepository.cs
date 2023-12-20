namespace Chirp.Infrastructure
{
    /// <summary>
    ///     Provides repository access to database
    /// </summary>
    public interface ICheepRepository
    {
        /// <summary>
        ///     Creates a Cheap for a specified Author in the database
        /// </summary>
        /// <param name="author"></param>
        /// <param name="text"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<Cheep> Create(Author author, string text, DateTime timestamp);

        /// <summary>
        ///     Fetches all Cheeps for specified page from database
        /// </summary>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<Cheep>> GetAll(int page, int skipCount, int takeCount);

        /// <summary>
        ///     Fetch Cheeps for specified Authors and Page from database 
        /// </summary>
        /// <param name="authorIds"></param>
        /// <param name="page"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        Task<IEnumerable<Cheep>> GetByAuthors(IEnumerable<Guid> authors, int page, int skipCount, int takeCount);


    }
}
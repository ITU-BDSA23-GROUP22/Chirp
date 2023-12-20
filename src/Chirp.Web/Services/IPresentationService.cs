using Chirp.Core;
using Chirp.Web.ViewModels;

namespace Chirp.Web
{
    /// <summary>
    ///     Encapsulates FrontEnd logic to work with Cheeps and Authors
    /// </summary>
    public interface IPresentationService
    {
        /// <summary>
        ///     Gets authenticated AuthorDTO from HttpContext.
        ///     The authenticated author is set by EnsureAuthorCreatedFilter
        /// </summary>
        /// <returns></returns>
        AuthorDTO? GetAuthenticatedAuthor();

        /// <summary>
        ///     Gets specified AuthorDTO from ChirpService
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<AuthorDTO> GetAuthor(Guid authorId);

        /// <summary>
        ///     Gets or Creates specified AuthorDTO from ChirpService.
        ///     If Author not already created, it will be created.
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="authorName"></param>
        /// <returns></returns>
        Task<AuthorDTO> GetOrCreateAuthor(Guid authorId, string authorName);

        /// <summary>
        ///     Gets CheepListViewModel for Cheep list from ChirpService
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<CheepListViewModel> GetAllCheepsViewModel(int pageNumber);

        /// <summary>
        ///     Gets CheepListViewModel for list of Cheeps from specified Authors from ChirpService.
        /// </summary>
        /// <param name="authorIds"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        Task<CheepListViewModel> GetCheepsByAuthorsViewModel(IEnumerable<Guid> authorIds, int pageNumber, string pageUrl);

        /// <summary>
        ///     Creates specified Cheep from ChirpService
        /// </summary>
        /// <param name="cheepText"></param>
        /// <returns></returns>
        Task CreateCheep(string cheepText);

        /// <summary>
        ///     Follows specified Author for authenticated author from ChirpService
        /// </summary>
        /// <param name="authorToFollowId"></param>
        /// <returns></returns>
        Task FollowAuthor(Guid authorToFollowId);

        /// <summary>
        ///     Unfollows specified Author for authenticated author from ChirpService
        /// </summary>
        /// <param name="authorToFollowId"></param>
        /// <returns></returns>
        Task UnfollowAuthor(Guid authorToFollowId);

        /// <summary>
        ///     Gets AuthorListViewModel for list of cheeps, based on specified search text from ChirpService
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<AuthorListViewModel> GetAuthorListViewModel(string? searchText, int pageNumber);

        /// <summary>
        ///     Gets specified Author's following Authors
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<IEnumerable<AuthorDTO>> GetFollowingAuthors(Guid authorId);

        /// <summary>
        ///     Deletes specified Author and anonymizes Cheeps from Author from ChirpService 
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task AnonymizeAuthor(Guid authorId);

        /// <summary>
        ///     Gets Cheeps and Followers for donwnload for specified Author from ChirpService
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<string> GetCheepsAndFollowerDownloadForAuthor(Guid authorId);
    }
}
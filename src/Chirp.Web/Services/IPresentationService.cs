using Chirp.Core;
using Chirp.Web.ViewModels;

namespace Chirp.Web
{
    public interface IPresentationService
    {
        AuthorDTO? GetAuthenticatedAuthor();

        Task<AuthorDTO> GetAuthor(Guid authorId);

        Task<AuthorDTO> GetOrCreateAuthor(Guid authorId, string authorName);

        Task<CheepListViewModel> GetAllCheepsViewModel(int pageNumber);

        Task<CheepListViewModel> GetCheepsByAuthorsViewModel(IEnumerable<Guid> authorIds, int pageNumber, string pageUrl);

        Task CreateCheep(string cheepText);

        Task FollowAuthor(Guid authorToFollowId);

        Task UnfollowAuthor(Guid authorToFollowId);

        Task<AuthorListViewModel> GetAuthorListViewModel(string? searchText, int pageNumber);


    }
}
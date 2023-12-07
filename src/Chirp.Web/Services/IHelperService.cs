using Chirp.Core;
using Chirp.Web.ViewModels;

namespace Chirp.Web
{
    public interface IHelperService
    {
        Task<Guid> GetAuthorId();

        Task<AuthorDTO?> GetAuthor();
        Task<AuthorDTO?> GetAuthor(Guid authorId);

        Task<CheepListViewModel> GetAllCheepsViewModel(int pageNumber);

        Task<CheepListViewModel> GetCheepsByAuthorsViewModel(IEnumerable<Guid> authorIds, int pageNumber, string pageUrl);

        Task CreateCheep(string cheepText);

        Task FollowAuthor(Guid authorToFollowId);

        Task UnfollowAuthor(Guid authorToFollowId);
    }
}
using Chirp.Core;

namespace Chirp.Web.ViewModels
{
    public class CheepListViewModel
    {
        public AuthorDTO? Author { get; }
        public IEnumerable<CheepViewModel> Cheeps { get; }

        public string PageUrl { get; }
        public int PageNumber { get; }
        public string NavigateToPreviousPageUrl { get; }
        public string NavigateToNextPageUrl { get; }

        public CheepListViewModel()
        {
            this.Author = null;
            this.Cheeps = Enumerable.Empty<CheepViewModel>();
        }

        public CheepListViewModel(AuthorDTO? author, IEnumerable<CheepDTO> cheeps, int pageNumber, int cheepsPerPage, string pageUrl)
        {
            this.Author = author;

            this.Cheeps = cheeps
                .Take(cheepsPerPage)
                .Select(cheepDto =>
            {
                var authorDto = cheepDto.Author;

                var authorViewModel = new AuthorViewModel(
                     authorDto.Id,
                     authorDto.Name,
                     this.CanFollow(this.Author, authorDto.Id),
                     this.CanUnfollow(this.Author, authorDto.Id)
                     );

                var cheepViewModel = new CheepViewModel(
                    cheepDto.Id,
                    cheepDto.Message,
                    cheepDto.Timestamp,
                    authorViewModel);

                return cheepViewModel;
            });

            this.PageUrl = pageUrl;
            this.PageNumber = pageNumber;
            this.NavigateToPreviousPageUrl = pageNumber > 1 ? $"{pageUrl}?page={pageNumber - 1}" : string.Empty;
            this.NavigateToNextPageUrl = cheeps.Count() > cheepsPerPage ? $"{pageUrl}?page={pageNumber + 1}" : string.Empty;
        }

        private bool CanFollow(AuthorDTO? authenticatedAuthor, Guid authorToFollowId)
        {
            if (authenticatedAuthor == null)
            {
                // Annonymous author
                return false;
            }
            if (authenticatedAuthor.Id == authorToFollowId)
            {
                // Cannot follow self
                return false;
            }
            if (authenticatedAuthor.followingIds.Contains(authorToFollowId))
            {
                // Already following
                return false;
            }

            return true;
        }

        private bool CanUnfollow(AuthorDTO? authenticatedAuthor, Guid authorToUnfollowId)
        {
            if (authenticatedAuthor == null)
            {
                // Annonymous author
                return false;
            }
            if (authenticatedAuthor.Id == authorToUnfollowId)
            {
                // Cannot unfollow self
                return false;
            }
            if (!authenticatedAuthor.followingIds.Contains(authorToUnfollowId))
            {
                // not following
                return false;
            }

            return true;
        }
    }
}
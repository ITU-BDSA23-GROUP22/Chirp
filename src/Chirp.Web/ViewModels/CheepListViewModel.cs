using Chirp.Core;

namespace Chirp.Web.ViewModels
{
    /// <summary>
    ///     Provides CheepListViewModel for markup code rendering list of cheeps.
    ///     Also, defines pagination buttons (prev & next)
    /// </summary>
    public class CheepListViewModel
    {
        public IEnumerable<CheepViewModel> Cheeps { get; }

        public string PageUrl { get; }
        public int PageNumber { get; }
        public string NavigateToPreviousPageUrl { get; }
        public string NavigateToNextPageUrl { get; }

        public CheepListViewModel()
        {
            this.Cheeps = Enumerable.Empty<CheepViewModel>();
            this.PageUrl = string.Empty;
            this.NavigateToNextPageUrl = string.Empty;
            this.NavigateToPreviousPageUrl = string.Empty;
        }

        public CheepListViewModel(AuthorDTO? authenticatedAuthor, IEnumerable<CheepDTO> cheeps, int pageNumber, int cheepsPerPage, string pageUrl)
        {
           

            this.Cheeps = cheeps
                .Take(cheepsPerPage)
                .Select(cheepDto =>
            {
                var authorDto = cheepDto.Author;

                var authorViewModel = new AuthorViewModel(
                     authorDto.Id,
                     authorDto.Name,
                     authenticatedAuthor
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
    }
}